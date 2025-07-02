using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Services.ImageService;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Controllers
{
    public class ClinicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _environment;
        private readonly IImageService _imageService;
        public ClinicController(ApplicationDbContext context, IHostEnvironment environment, IImageService imageService)
        {
            _context = context;
            _environment = environment;
            _imageService = imageService;
        }
        public IActionResult Index()
        { 

           
            return View();
        }
    }
}
