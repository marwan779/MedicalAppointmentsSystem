using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.User.Controllers
{
    [Area("User")]
    public class UserAppointmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
