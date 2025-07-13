using System.Security.Claims;
using MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels.UserAppointmentResponse;
using MedicalAppointmentsSystem.Areas.User.Models;
using MedicalAppointmentsSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = StaticDetails.DoctorRole)]
    public class UserAppointmentResponsesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAppointmentResponsesController(ApplicationDbContext context)
        {
            _context = context;
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
                foreach(var userAppointmentRequest in userAppointmentRequests)
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
    }
}
