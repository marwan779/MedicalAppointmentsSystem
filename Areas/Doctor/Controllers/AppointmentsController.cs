using System.Security.Claims;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = StaticDetails.DoctorRole)]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            //List<DoctorAvailability> doctorAvailabilities = _context.DoctorAvailabilities.Where(a => a.DoctorId == UserId && a.IsBooked).ToList();



            return View();
        }
    }
}
