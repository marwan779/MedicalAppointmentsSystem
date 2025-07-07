using System.Diagnostics;
using MedicalAppointmentsSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Controllers
{
    [Area("Doctor")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
