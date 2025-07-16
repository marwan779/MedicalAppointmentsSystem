using MedicalAppointmentsSystem.Areas.User.Controllers;
using MedicalAppointmentsSystem.Areas.User.Models.ViewModels;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Services.ImageService;
using MedicalAppointmentsSystem.Services.MailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.AdminRole)]
    public class UserComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;
        public UserComplaintsController(ApplicationDbContext context, IFileService fileService, IWebHostEnvironment webHostEnvironment, IEmailService emailService)
        {
            _context = context;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            List<UserComplaint> userComplaints = _context.UserComplaints.ToList();


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
        public IActionResult GetComplaintBody(int id)
        {
            var complaint = _context.UserComplaints.FirstOrDefault(c => c.Id == id);
            if (complaint == null)
            {
                return Content("<div class='alert alert-danger'>Complaint not found</div>");
            }
            return Content(complaint.ComplaintBody);
        }

        [HttpGet]
        public IActionResult ForwardComplaint(int id)
        {
            var complaint = _context.UserComplaints.FirstOrDefault(c => c.Id == id);

            ApplicationUser? applicationUser = _context.ApplicationUsers
                .FirstOrDefault(u => u.Id == complaint.DoctorId);

            MailData mailData = new MailData()
            {
                EmailToId = applicationUser.Email,
                EmailToName = applicationUser.FullName,
                EmailSubject = "Complaint Filed - Case #" + complaint.ComplaintTitle,
                EmailBody = $@"Dear Dr. {applicationUser.FullName},

                A new patient complaint has been filed regarding your practice at {_context.Clinics.FirstOrDefault(c => c.Id == complaint.ClinicId)?.Name}:

                Case #: {complaint.Id}
                Title: {complaint.ComplaintTitle}
                Filed On: {complaint.FiledAt.ToString("MMMM dd, yyyy")}
                Patient: {_context.ApplicationUsers.FirstOrDefault(u => u.Id == complaint.UserId)?.FullName}

                Action Required:
                1. Please review this complaint carefully
                2. Prepare a response within 3 business days
                3. Coordinate with our admin team for resolution

                You can view the full complaint details by logging into the Medical Appointments System.

                This is an automated notification. Please do not reply directly to this email.

                Medical Appointments System Administration"
            };

            _emailService.SendMail(mailData);

            complaint.ComplaintStatus = StaticDetails.DoctorInformed;

            _context.UserComplaints.Update(complaint);
            _context.SaveChanges();

            TempData["Success"] = "The complaint is sent to the doctor successfully.";
            return RedirectToAction(nameof(Index));

        }


    }
}
