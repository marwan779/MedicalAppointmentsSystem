using System.Security.Claims;
using MedicalAppointmentsSystem.Areas.User.Models;
using MedicalAppointmentsSystem.Areas.User.Models.ViewModels;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Models.ViewModels.Clinic;
using MedicalAppointmentsSystem.Services.ImageService;
using MedicalAppointmentsSystem.Services.MailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = StaticDetails.UserRole)]
    public class UserAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;

        public UserAppointmentsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment,
            IFileService fileService, IEmailService emailService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<UserAppointmentRequest>? requests = _context.userAppointmentRequests.Where(r => r.UserId == userId).ToList();

            List<UserAppointmentRequestVM> requestVMs = new List<UserAppointmentRequestVM>();

            if (requests.Any())
            {
                foreach (var request in requests)
                {
                    Clinic clinic = _context.Clinics.FirstOrDefault(c => c.Id == request.ClinicId);
                    requestVMs.Add
                        (
                            new UserAppointmentRequestVM()
                            {
                                Id = request.Id,
                                ClinicName = clinic.Name,
                                DoctorName = _context.ApplicationUsers.FirstOrDefault(u => u.Id == clinic.DoctorId).FullName,
                                RequestStatus = request.RequestStatus,
                                PreferredDate = request.PreferredDate,
                                RequestedAt = request.RequestedAt,

                            }

                        );
                }
            }

            return View(requestVMs);
        }

        [HttpGet]
        public IActionResult RequestAnAppointment(int clinicId)
        {
            ViewBag.ClinicId = clinicId;
            return View();
        }

        [HttpPost]
        public IActionResult RequestAnAppointment(SetUserAppointmentRequestVM setUserAppointmentRequest, int clinicId)
        {
            if (!ModelState.IsValid)
            {
                return View(setUserAppointmentRequest);
            }

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);
            ApplicationUser? doctor = _context.ApplicationUsers.FirstOrDefault(d => d.Id == clinic.DoctorId);

            UserAppointmentRequest userAppointmentRequest = new UserAppointmentRequest()
            {
                UserId = userId,
                ClinicId = clinicId,
                DoctorId = doctor.Id,
                ComplainsAbout = setUserAppointmentRequest.ComplainsAbout,
                ChronicDiseases = setUserAppointmentRequest.ChronicDiseases,
                PreferredDate = setUserAppointmentRequest.PreferredDate,
            };

            _context.userAppointmentRequests.Add(userAppointmentRequest);
            _context.SaveChanges();


            if (setUserAppointmentRequest.Document != null)
            {
                if (Path.GetExtension(setUserAppointmentRequest.Document.FileName) != ".pdf")
                {
                    TempData["Error"] = "Invalid document type. Only .pdf is allowed.";
                    return View(setUserAppointmentRequest);
                }


                string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents", "UserRequests", userId);

                Directory.CreateDirectory(folderPath);

                userAppointmentRequest.DocumentsUrl = _fileService.SaveFile(setUserAppointmentRequest.Document, folderPath, @"\Documents\UserRequests\" + userId + "\\");

                _context.userAppointmentRequests.Update(userAppointmentRequest);
                _context.SaveChanges();

            }


            MailData mailData = new MailData()
            {
                EmailToId = doctor.Email,
                EmailToName = "Dr. " + doctor.FullName,
                EmailSubject = "Patient Requests An Appointment",
                EmailBody = $"Dear Dr. {doctor.FullName},\n\n" +
                $"A patient has requested an appointment in {clinic.Name} clinic.\n\n" +
                "Please log in to your account and visit the Appointments Management page to review and schedule this appointment at your earliest convenience.\n\n" +
                "Thank you for using our services.\n\n" +
                "Best regards,\n" +
                "Medical Appointments System"
            };

            _emailService.SendMail(mailData);

            TempData["Success"] = "Appointment request is sent to the doctor successfully, You will receive an email once the doctor schedules your appointment.    ";
            return RedirectToAction(nameof(Index));


        }
    }
}
