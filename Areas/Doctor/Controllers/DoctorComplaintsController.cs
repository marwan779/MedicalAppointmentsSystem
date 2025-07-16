using System.Security.Claims;
using MedicalAppointmentsSystem.Areas.Doctor.Models;
using MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels;
using MedicalAppointmentsSystem.Areas.User.Models.ViewModels;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Services.ImageService;
using MedicalAppointmentsSystem.Services.MailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = StaticDetails.DoctorRole)]  
    public class DoctorComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;
        public DoctorComplaintsController(ApplicationDbContext context, IFileService fileService, IWebHostEnvironment webHostEnvironment, IEmailService emailService)
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
                .Where(c => c.DoctorId == userId).ToList();

            List <DoctorComplaintVM> complaintVMs = new List<DoctorComplaintVM>();

            if(userComplaints.Any())
            {
                foreach (var userComplaint in userComplaints)
                {
                    complaintVMs.Add
                        (
                            new DoctorComplaintVM()
                            {
                                Id = userComplaint.Id,
                                ComplaintTitle = userComplaint.ComplaintTitle,
                                FiledAt = userComplaint.FiledAt,
                                ClinicName = _context.Clinics.FirstOrDefault(c => c.Id == userComplaint.ClinicId).Name,
                                DocumentUrl = userComplaint.DocumentUrl,
                                DoctorResponded = userComplaint.DoctorResponded,
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
        public IActionResult RespondToComplaint(int id)
        {
            ViewBag.ComplaintId = id;

            return View();
        }

        [HttpPost]
        public IActionResult RespondToComplaint(DoctorComplaintResponseVM doctorComplaintResponse)
        {
            if (!ModelState.IsValid)
            {
                return View(doctorComplaintResponse);
            }

            UserComplaint? userComplaint = _context.UserComplaints
                .FirstOrDefault(c => c.Id == doctorComplaintResponse.ComplaintId);

            userComplaint.DoctorReponse = doctorComplaintResponse.Response;
            userComplaint.ComplaintStatus = StaticDetails.DoctorResponded;
            userComplaint.DoctorResponded = true;

            _context.UserComplaints.Update(userComplaint);
            _context.SaveChanges();

            ApplicationUser? applicationUser = _context.ApplicationUsers
                .FirstOrDefault(u => u.Id == userComplaint.UserId);

            MailData mailData = new MailData()
            {
                EmailToId = applicationUser.Email,
                EmailToName = applicationUser.FullName,
                EmailSubject = "Complaint Response - Case #" + userComplaint.ComplaintTitle,
                EmailBody = $@"Dear {applicationUser.FullName},

                We would like to inform you that the doctor has responded to your complaint regarding:
                <b>'{userComplaint.ComplaintTitle}'</b>

                You can view the doctor's response by visiting the Complaint Management page in our system.

                <b>Next Steps:</b>
                1. Log in to your account
                2. Navigate to 'My Complaints' section
                3. View the details of this complaint to see the doctor's response

                If you have any further questions or need additional assistance, please don't hesitate to contact our support team.

                Thank you for helping us improve our services.

                Best regards,
                Medical Appointments System Administration"
            };

            bool res = _emailService.SendMail(mailData);


            TempData["Success"] = "Your response is sent successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
