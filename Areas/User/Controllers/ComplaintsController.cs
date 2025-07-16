using System.Security.Claims;
using MedicalAppointmentsSystem.Areas.User.Models.ViewModels;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Services.ImageService;
using MedicalAppointmentsSystem.Services.MailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalAppointmentsSystem.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = StaticDetails.UserRole)]
    public class ComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;
        public ComplaintsController(ApplicationDbContext context, IFileService fileService, IWebHostEnvironment webHostEnvironment, IEmailService emailService)
        {
            _context = context;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<UserComplaint> userComplaints = _context.UserComplaints
                .Where(c => c.UserId == userId).ToList();

            List<ComplaintVM> complaintVMs = new List<ComplaintVM>();

            if (userComplaints.Any())
            {
                foreach (var userComplaint in userComplaints)
                {
                    complaintVMs.Add
                        (
                            new ComplaintVM()
                            {
                                Id = userComplaint.Id,
                                ComplaintTitle = userComplaint.ComplaintTitle,
                                ComplaintStatus = userComplaint.ComplaintStatus,
                                FiledAt = userComplaint.FiledAt,
                                ClinicName = _context.Clinics.FirstOrDefault(c => c.Id == userComplaint.ClinicId).Name,
                                DocumentUrl = userComplaint.DocumentUrl,
                            }

                        );
                }
            }


            return View(complaintVMs);
        }

        [HttpGet]
        public IActionResult FileComplaint()
        {
            List<Clinic> clinics = _context.Clinics.ToList();

            IEnumerable<SelectListItem> ClinicsList = clinics.Select
                (
                    c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }

                );

            ViewBag.ClinicsList = ClinicsList;

            return View();
        }

        [HttpPost]
        public IActionResult FileComplaint(AddComplaintVM addComplaintVM)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == addComplaintVM.ClinicId);

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic";
                return RedirectToAction(nameof(Index));
            }


            UserComplaint userComplaint = new UserComplaint()
            {
                ClinicId = addComplaintVM.ClinicId,
                ComplaintBody = addComplaintVM.ComplaintBody,
                UserId = userId,
                DoctorId = clinic.DoctorId,
                ComplaintTitle = addComplaintVM.ComplaintTitle,
            };

            if (addComplaintVM.Document != null)
            {
                if (Path.GetExtension(addComplaintVM.Document.FileName) != ".pdf")
                {
                    TempData["Error"] = "Invalid document type. Only .pdf is allowed.";
                    return RedirectToAction(nameof(Index));
                }

                string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents", "UserComplaints", userId);
                Directory.CreateDirectory(folderPath);

                userComplaint.DocumentUrl = _fileService.SaveFile(addComplaintVM.Document, folderPath, @"\Documents\UserComplaints\" + userId + "\\");
            }

            _context.UserComplaints.Add(userComplaint);
            _context.SaveChanges();

            ApplicationUser? applicationUser = _context.ApplicationUsers
                .FirstOrDefault(u => u.Id == userId);

            MailData mailData = new MailData()
            {
                EmailToId = applicationUser.Email,
                EmailToName = applicationUser.FullName,
                EmailSubject = "Complaint Received - Case #" + addComplaintVM.ComplaintTitle,
                EmailBody = $@"Dear {applicationUser.FullName},

                We acknowledge receipt of your complaint (Case #: {addComplaintVM.ComplaintTitle}) and want to assure you 
                that it has been forwarded to our support team for review.

                One of our administrators will carefully examine your concern and respond to you 
                within 2-3 business days.

                For your reference, please keep this case number for any future communication 
                regarding this matter.

                Thank you for bringing this to our attention.

                Best regards,
                Medical Appointments System Support Team"
            };

            _emailService.SendMail(mailData);

            TempData["Success"] = "Your complaint is sent successfully, One of our administrators will review it and contact you shortly.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult WithdrawComplaint(int complaintId)
        {

            UserComplaint? userComplaint = _context.UserComplaints.FirstOrDefault(c => c.Id == complaintId);

            if (userComplaint == null)
            {
                TempData["Error"] = "No such complaint";
                return RedirectToAction(nameof(Index));
            }

            if(!String.IsNullOrEmpty(userComplaint.DocumentUrl))
            {
                _fileService.DeleteFile(userComplaint.DocumentUrl);
                userComplaint.DocumentUrl = string.Empty;
            }

            userComplaint.ComplaintTitle = "-";
            userComplaint.ComplaintBody = "-";
            userComplaint.ComplaintStatus = StaticDetails.ComplaintWithdrawn;

            _context.UserComplaints.Update(userComplaint);

            _context.SaveChanges();

            ApplicationUser? applicationUser = _context.ApplicationUsers
                .FirstOrDefault(u => u.Id == userComplaint.UserId);

            MailData mailData = new MailData()
            {
                EmailToId = applicationUser.Email,
                EmailToName = applicationUser.FullName,
                EmailSubject = "Complaint Withdrawn",
                EmailBody = $@"Dear {applicationUser.FullName},

                Your Complaint Is Withdrawn Successfully.

                Best regards,
                Medical Appointments System Support Team"
            };

            _emailService.SendMail(mailData);

            TempData["Success"] = "Your complaint is withdrawn successfully.";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult GetComplaintResponse(int id)
        {
            var complaint = _context.UserComplaints.FirstOrDefault(c => c.Id == id);
            if (complaint == null)
            {
                return Content("<div class='alert alert-danger'>Complaint not found</div>");
            }
            return Content(complaint.DoctorReponse);
        }

    }
}
