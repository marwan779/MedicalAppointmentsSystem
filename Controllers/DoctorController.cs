using System.Security.Claims;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Services.ImageService;
using MedicalAppointmentsSystem.Views.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Doctor.Controllers
{
    [Authorize(Roles = StaticDetails.DoctorRole)]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;
        public DoctorController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            DoctorInformation? doctorInformation = _context.DoctorInformation.FirstOrDefault(d => d.UserId == UserId);
            if (doctorInformation == null)
            {
                TempData["Error"] = "Doctor's Inofrmation is not added yet";
                return RedirectToAction(nameof(AddDoctorInformation));
            }

            DoctorInformationVM doctorInformationVM = new DoctorInformationVM()
            {
                LicenseNumber = doctorInformation.LicenseNumber,
                Specialization = doctorInformation.Specialization,
                YearsExperience = doctorInformation.YearsExperience,
                MedicalSchool = doctorInformation.MedicalSchool,
                GraduationYear = doctorInformation.GraduationYear,
                HospitalAffiliation = doctorInformation.HospitalAffiliation,
                Department = doctorInformation.Department,
                Position = doctorInformation.Position,
                Bio = doctorInformation.Bio,
                EmergencyPhone = doctorInformation.EmergencyPhone,
                LicenseImagePath = doctorInformation.LicenseImagePath,
                IdProofPath = doctorInformation.IdProofPath,
                PhotoPath = doctorInformation.PhotoPath,
                RegistrationDate = doctorInformation.RegistrationDate,
                DoctorName = _context.ApplicationUsers.FirstOrDefault(n => n.Id == UserId).FullName
            };



            return View(doctorInformationVM);
        }


        [HttpGet]
        public IActionResult AddDoctorInformation()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddDoctorInformation([FromForm] AddDoctorInformationVM addDoctorInformationVM)
        {


            if (!ModelState.IsValid)
            {

                return View(addDoctorInformationVM);
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            DoctorInformation doctorInformation = new DoctorInformation()
            {
                UserId = UserId,
                LicenseNumber = addDoctorInformationVM.LicenseNumber,
                Specialization = addDoctorInformationVM.Specialization,
                Position = addDoctorInformationVM.Position,
                Bio = addDoctorInformationVM.Bio,
                Department = addDoctorInformationVM.Department,
                GraduationYear = addDoctorInformationVM.GraduationYear,
                EmergencyPhone = addDoctorInformationVM.EmergencyPhone,
                MedicalSchool = addDoctorInformationVM.MedicalSchool,
                HospitalAffiliation = addDoctorInformationVM.HospitalAffiliation,
                YearsExperience = addDoctorInformationVM.YearsExperience,
            };


            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Doctors", UserId);

            Directory.CreateDirectory(folderPath);


            doctorInformation.IdProofPath = _imageService.SaveImage(addDoctorInformationVM.IdProof, folderPath, UserId);
            doctorInformation.PhotoPath = _imageService.SaveImage(addDoctorInformationVM.Photo, folderPath, UserId);
            doctorInformation.LicenseImagePath = _imageService.SaveImage(addDoctorInformationVM.LicenseImage, folderPath, UserId);

            if (String.IsNullOrEmpty(doctorInformation.LicenseImagePath) ||
                String.IsNullOrEmpty(doctorInformation.PhotoPath) ||
                String.IsNullOrEmpty(doctorInformation.IdProofPath))
            {
                TempData["Error"] = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
                return RedirectToAction(nameof(AddDoctorInformation));
            }

            _context.DoctorInformation.Add(doctorInformation);




            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
