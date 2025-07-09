using System.Security.Claims;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Models.ViewModels.Doctor;
using MedicalAppointmentsSystem.Services.ImageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalAppointmentsSystem.Areas.Doctor.Controllers
{
    [Authorize(Roles = StaticDetails.DoctorRole)]
    [Area("Doctor")]

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
                TempData["Warning"] = "Doctor profile is not added yet";
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
                DoctorName = doctorInformation.DoctorName
            };



            return View(doctorInformationVM);
        }


        [HttpGet]
        public IActionResult AddDoctorInformation()
        {
            AddDoctorInformationVM addDoctorInformationVM = new AddDoctorInformationVM();

            addDoctorInformationVM.SpecializationList = StaticDetails.MedicalSpecializations
                .Select(i => i).Select(i => new SelectListItem()
                {
                    Text = i,
                    Value = i
                });

            return View(addDoctorInformationVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddDoctorInformation([FromForm] AddDoctorInformationVM addDoctorInformationVM)
        {

            if (!ModelState.IsValid)
            {
                addDoctorInformationVM.SpecializationList = StaticDetails.MedicalSpecializations
               .Select(i => i).Select(i => new SelectListItem()
               {
                   Text = i,
                   Value = i
               });
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
                DoctorName = _context.ApplicationUsers.FirstOrDefault(d => d.Id == UserId).FullName
            };


            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Doctors", UserId);

            Directory.CreateDirectory(folderPath);


            doctorInformation.IdProofPath = _imageService.SaveImage(addDoctorInformationVM.IdProof, folderPath, @"\Images\Doctors\" + UserId + "\\");
            doctorInformation.PhotoPath = _imageService.SaveImage(addDoctorInformationVM.Photo, folderPath, @"\Images\Doctors\" + UserId + "\\");
            doctorInformation.LicenseImagePath = _imageService.SaveImage(addDoctorInformationVM.LicenseImage, folderPath, @"\Images\Doctors\" + UserId + "\\");

            if (String.IsNullOrEmpty(doctorInformation.LicenseImagePath) ||
                String.IsNullOrEmpty(doctorInformation.PhotoPath) ||
                String.IsNullOrEmpty(doctorInformation.IdProofPath))
            {
                TempData["Error"] = "Invalid image type. Only .jpg, .jpeg, and .png files are allowed.";
                return RedirectToAction(nameof(AddDoctorInformation));
            }

            _context.DoctorInformation.Add(doctorInformation);

            _context.SaveChanges();

            TempData["Success"] = "Doctor profile is added successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult EditDoctorInformation()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            DoctorInformation? doctorInformation = _context.DoctorInformation.FirstOrDefault(d => d.UserId == userId);

            EditDoctorInformationVM editDoctorInformationVM = new EditDoctorInformationVM()
            {
                LicenseNumber = doctorInformation.LicenseNumber,
                YearsExperience = doctorInformation.YearsExperience,
                MedicalSchool = doctorInformation.MedicalSchool,
                Specialization = doctorInformation.Specialization,
                GraduationYear = doctorInformation.GraduationYear,
                HospitalAffiliation = doctorInformation.HospitalAffiliation,
                Department = doctorInformation.Department,
                Position = doctorInformation.Position,
                Bio = doctorInformation.Bio,
                EmergencyPhone = doctorInformation.EmergencyPhone,
                DoctorName = _context.ApplicationUsers.FirstOrDefault(n => n.Id == userId).FullName,
                PhotoPath = doctorInformation.PhotoPath,
                IdProofPath = doctorInformation.IdProofPath,
                LicenseImagePath = doctorInformation.LicenseImagePath,
                SpecializationList = StaticDetails.MedicalSpecializations
                .Select(i => i).Select(i => new SelectListItem()
                {
                    Text = i,
                    Value = i
                })
            };


            return View(editDoctorInformationVM);
        }

        [HttpPost]
        public IActionResult EditDoctorInformation(EditDoctorInformationVM editDoctorInformationVM)
        {
            if (!ModelState.IsValid)
            {
                editDoctorInformationVM.SpecializationList = StaticDetails.MedicalSpecializations
                .Select(i => i).Select(i => new SelectListItem()
                {
                    Text = i,
                    Value = i
                });
                return View(editDoctorInformationVM);
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            DoctorInformation? doctorInformation = _context.DoctorInformation.FirstOrDefault(d => d.UserId == UserId);


            doctorInformation.LicenseNumber = editDoctorInformationVM.LicenseNumber;
            doctorInformation.Specialization = editDoctorInformationVM.Specialization;
            doctorInformation.Position = editDoctorInformationVM.Position;
            doctorInformation.Bio = editDoctorInformationVM.Bio;
            doctorInformation.Department = editDoctorInformationVM.Department;
            doctorInformation.GraduationYear = editDoctorInformationVM.GraduationYear;
            doctorInformation.EmergencyPhone = editDoctorInformationVM.EmergencyPhone;
            doctorInformation.MedicalSchool = editDoctorInformationVM.MedicalSchool;
            doctorInformation.HospitalAffiliation = editDoctorInformationVM.HospitalAffiliation;
            doctorInformation.YearsExperience = editDoctorInformationVM.YearsExperience;


            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Doctors", UserId);

            if (editDoctorInformationVM.IdProof != null)
                doctorInformation.IdProofPath = _imageService.UpdateImage(editDoctorInformationVM.IdProof, editDoctorInformationVM.IdProofPath, folderPath, @"\Images\Doctors\" + UserId + "\\");

            if (editDoctorInformationVM.Photo != null)
                doctorInformation.PhotoPath = _imageService.UpdateImage(editDoctorInformationVM.Photo, editDoctorInformationVM.PhotoPath, folderPath, @"\Images\Doctors\" + UserId + "\\");

            if (editDoctorInformationVM.LicenseImage != null)
                doctorInformation.LicenseImagePath = _imageService.UpdateImage(editDoctorInformationVM.LicenseImage, editDoctorInformationVM.LicenseImagePath, folderPath, @"\Images\Doctors\" + UserId + "\\");

            if (String.IsNullOrEmpty(doctorInformation.LicenseImagePath) ||
                String.IsNullOrEmpty(doctorInformation.PhotoPath) ||
                String.IsNullOrEmpty(doctorInformation.IdProofPath))
            {
                TempData["Error"] = "Invalid image type. Only .jpg, .jpeg, and .png files are allowed.";
                return RedirectToAction(nameof(EditDoctorInformation));
            }

            _context.Update(doctorInformation);
            _context.SaveChanges();

            TempData["Success"] = "Doctor profile is updated successfully";
            return RedirectToAction(nameof(Index));
        }
          
    }
}
