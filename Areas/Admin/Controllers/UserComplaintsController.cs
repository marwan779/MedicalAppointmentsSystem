using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Admin.Controllers
{
    public class UserComplaintsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FileComplaint()
        {
            return View();    
        }


    }
}
