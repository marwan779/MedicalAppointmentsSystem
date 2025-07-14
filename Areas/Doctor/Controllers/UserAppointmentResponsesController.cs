using System.Globalization;
using System.Security.Claims;
using MedicalAppointmentsSystem.Areas.Doctor.Models;
using MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels.UserAppointmentResponse;
using MedicalAppointmentsSystem.Areas.User.Models;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Models.ViewModels.Appointments;
using MedicalAppointmentsSystem.Services.ImageService;
using MedicalAppointmentsSystem.Services.MailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalAppointmentsSystem.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = StaticDetails.DoctorRole)]
    public class UserAppointmentResponsesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;

        public UserAppointmentResponsesController(ApplicationDbContext context, IEmailService emailService, IFileService fileService)
        {
            _context = context;
            _emailService = emailService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<UserAppointmentRequest> userAppointmentRequests = _context.userAppointmentRequests
                .Where(r => r.DoctorId == userId).ToList();

            List<UserAppointmentResponseVM> userAppointmentResponseVMs = new List<UserAppointmentResponseVM>();

            if (userAppointmentRequests.Any())
            {
                foreach (var userAppointmentRequest in userAppointmentRequests)
                {
                    userAppointmentResponseVMs.Add
                        (
                            new UserAppointmentResponseVM()
                            {
                                Id = userAppointmentRequest.Id,
                                UserId = userAppointmentRequest.UserId,
                                DoctorId = userId,
                                UserName = _context.ApplicationUsers.FirstOrDefault(u => u.Id == userAppointmentRequest.UserId).FullName,
                                ClinicId = userAppointmentRequest.ClinicId,
                                RequestedAt = userAppointmentRequest.RequestedAt,
                                PreferredDate = userAppointmentRequest.PreferredDate,
                                DocumentsUrl = userAppointmentRequest.DocumentsUrl,
                                RequestStatus = userAppointmentRequest.RequestStatus,
                                ComplainsAbout = userAppointmentRequest.ComplainsAbout,
                                ChronicDiseases = userAppointmentRequest.ChronicDiseases,
                            }
                        );
                }
            }

            return View(userAppointmentResponseVMs);
        }


        [HttpGet]
        public IActionResult ScheduleAppointment(int requestId)
        {
            UserAppointmentRequest? userAppointmentRequest = _context.userAppointmentRequests
                .FirstOrDefault(r => r.Id == requestId);

            if (userAppointmentRequest == null)
            {
                TempData["Error"] = "No Such Request";
                return RedirectToAction(nameof(Index));
            }

            List<DoctorAvailability> doctorAvailabilities = _context.DoctorAvailabilities
                .Where(d => d.DoctorId == userAppointmentRequest.DoctorId).ToList();

            List<DoctorAvailabilityVM> doctorAvailabilityVMs = new List<DoctorAvailabilityVM>();



            if (doctorAvailabilities.Any())
            {
                foreach (var item in doctorAvailabilities)
                {
                    doctorAvailabilityVMs.Add
                        (
                            new DoctorAvailabilityVM()
                            {
                                Id = item.Id,
                                StartTime = item.StartTime,
                                EndTime = item.EndTime,
                                Day = item.Day,
                            }
                        );
                }

                IEnumerable<SelectListItem> doctorAvailableDays = doctorAvailabilities
                    .Select(d => new SelectListItem()
                    {
                        Text = d.Day,
                        Value = d.Day,
                    });

                ViewBag.DoctorAvailableDays = doctorAvailableDays;
            }

            ViewBag.DoctorAvailability = doctorAvailabilityVMs;
            ViewBag.RequestId = requestId;

            return View();
        }

        [HttpPost]
        public IActionResult ScheduleAppointment(ScheduleUserAppointmentVM scheduleUserAppointmentVM, int requestId)
        {

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var doctorId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!ModelState.IsValid)
            {

                return RedirectToAction(nameof(Index));
            }

            DoctorAvailability? doctorAvailability = _context.DoctorAvailabilities
                .FirstOrDefault(d => d.DoctorId == doctorId
                                && d.Day == scheduleUserAppointmentVM.AppointmentDay);

            if (scheduleUserAppointmentVM.AppointmentStartTime < doctorAvailability.StartTime
               || scheduleUserAppointmentVM.AppointmentStartTime > doctorAvailability.EndTime)
            {
                TempData["Error"] = "Doctor is not available at this time";
                return RedirectToAction(nameof(Index));
            }

            DateTime dateTime = scheduleUserAppointmentVM.AppointmentDate.ToDateTime(TimeOnly.MinValue);

            string day = dateTime.ToString("dddd", CultureInfo.InvariantCulture);

            if (!string.Equals(day, scheduleUserAppointmentVM.AppointmentDay, StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Appointment Date and Appointment Day does not match";
                return RedirectToAction(nameof(Index));
            }


            UserAppointmentRequest? userAppointmentRequest = _context.userAppointmentRequests
                .FirstOrDefault(r => r.Id == requestId);

            UserAppointmentResponse userAppointmentResponse = new UserAppointmentResponse()
            {
                AppointmentDay = scheduleUserAppointmentVM.AppointmentDay,
                DoctorId = doctorId,
                ClinicId = userAppointmentRequest.ClinicId,
                UserId = userAppointmentRequest.UserId,
                AppointmentDate = scheduleUserAppointmentVM.AppointmentDate,
                AppointmentStartTime = scheduleUserAppointmentVM.AppointmentStartTime,
                AppointmentLocation = _context.Clinics.FirstOrDefault(c => c.Id == userAppointmentRequest.ClinicId).Location,
                RequestId = requestId,
            };

            _context.UserAppointmentResponses.Add(userAppointmentResponse);

            userAppointmentRequest.RequestStatus = StaticDetails.AppointmentScheduled;

            _context.userAppointmentRequests.Update(userAppointmentRequest);

            _context.SaveChanges();

            ApplicationUser? applicationUser = _context.ApplicationUsers
                .FirstOrDefault(u => u.Id == userAppointmentRequest.UserId);

            MailData mailData = new MailData()
            {
                EmailToId = applicationUser.Email,
                EmailToName = applicationUser.FullName,
                EmailSubject = "Appointment Scheduled",
                EmailBody = $@"Dear {applicationUser.FullName},

                Your appointment has been successfully scheduled.

                Please visit your appointment management page on our website to view all the details 
                about your upcoming appointment.

                If you have any questions or need to make changes to your appointment, 
                please don't hesitate to contact us.

                Thank you for choosing our services.

                Best regards,
                Medical Appointments System"
            };

            _emailService.SendMail(mailData);


            TempData["Success"] = "Appointment Scheduled Successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DoctorAppointmentDetails(int requestId)
        {
            UserAppointmentResponse? userAppointmentResponse = _context.UserAppointmentResponses
                .FirstOrDefault(r => r.RequestId == requestId);


            if (userAppointmentResponse == null)
            {
                if (userAppointmentResponse == null)
                {
                    ViewData["Error"] = "No Such Request";
                    return RedirectToAction(nameof(Index));
                }
            }



            DoctorAppointmentResponseVM AppointmentResponseVM = new DoctorAppointmentResponseVM()
            {
                Date = userAppointmentResponse.AppointmentDate,
                Day = userAppointmentResponse.AppointmentDay,
                Time = userAppointmentResponse.AppointmentStartTime,
                Location = userAppointmentResponse.AppointmentLocation
            };

            return View(AppointmentResponseVM);
        }

        [HttpGet]
        public IActionResult RejectRequest(int requestId)
        {
            UserAppointmentRequest? userAppointmentRequest = _context.userAppointmentRequests
                .FirstOrDefault(r => r.Id == requestId);

            if (userAppointmentRequest == null)
            {
                TempData["Error"] = "No Such Appointment";
                return RedirectToAction(nameof(Index));
            }

            userAppointmentRequest.RequestStatus = StaticDetails.RequestRejected;
            userAppointmentRequest.ChronicDiseases = "-";
            userAppointmentRequest.ComplainsAbout = "-";

            _fileService.DeleteFile(userAppointmentRequest.DocumentsUrl);


            userAppointmentRequest.DocumentsUrl = string.Empty;



            _context.userAppointmentRequests.Update(userAppointmentRequest);
            _context.SaveChanges();

            ApplicationUser? applicationUser = _context.ApplicationUsers
               .FirstOrDefault(u => u.Id == userAppointmentRequest.UserId);

            MailData mailData = new MailData()
            {
                EmailToId = applicationUser.Email,
                EmailToName = applicationUser.FullName,
                EmailSubject = "Appointment Scheduled",
                EmailBody = $@"Dear {applicationUser.FullName},

                We regret to inform you that your appointment request has been

                rejected due to doctor's full schedule 

                You can visit website to place another request

                Best regards,
                Medical Appointments System"
            };

            _emailService.SendMail(mailData);

            TempData["Success"] = "Appointment Rejected Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
