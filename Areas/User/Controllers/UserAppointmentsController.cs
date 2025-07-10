using System.Security.Claims;
using MedicalAppointmentsSystem.Areas.User.Models;
using MedicalAppointmentsSystem.Areas.User.Models.ViewModels;
using MedicalAppointmentsSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles =StaticDetails.UserRole)]
    public class UserAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<UserAppointmentRequest>? requests = _context.userAppointmentRequests.Where(r => r.UserId == userId).ToList();
            return View(requests);
        }

        [HttpGet]
        public IActionResult RequestAnAppointment(int clinicId)
        {
            ViewBag.ClinicId = clinicId;
            return View();
        }

        [HttpPost]
        public IActionResult RequestAnAppointment(UserAppointmentRequestVM userAppointmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(userAppointmentRequest);
            }

        }
    }
}
