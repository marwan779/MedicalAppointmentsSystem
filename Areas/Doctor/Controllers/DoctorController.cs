using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Doctor.Controllers
{
    [Authorize(Roles = StaticDetails.DoctorRole)]
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDoctorInFormation()
        {
        }
    }
}
