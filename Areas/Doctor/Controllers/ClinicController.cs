using System.Security.Claims;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Models.ViewModels.Clinic;
using MedicalAppointmentsSystem.Models.ViewModels.Doctor;
using MedicalAppointmentsSystem.Services.ImageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MedicalAppointmentsSystem.Areas.Controllers
{
    [Authorize(Roles = StaticDetails.DoctorRole)]
    [Area("Doctor")]
    public class ClinicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        public ClinicController(ApplicationDbContext context, IWebHostEnvironment environment, IFileService fileService)
        {
            _context = context;
            _webHostEnvironment = environment;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            DoctorInformation? doctorInformation = _context.DoctorInformation
                .FirstOrDefault(d => d.UserId == userId);

            if (doctorInformation == null)
            {
                TempData["Warning"] = "Doctor profile needs to be added first";
                return RedirectToAction("Index", "DoctorController");
            }

            List<Clinic> clinics = _context.Clinics.Where(c => c.DoctorId == userId).ToList();

            List<ClinicVM> clinicVMs = new List<ClinicVM>();

            foreach (var clinic in clinics)
            {
                clinicVMs.Add(new ClinicVM
                {
                    Name = clinic.Name,
                    Id = clinic.Id,
                    Location = clinic.Location,
                    PhoneNumber = clinic.PhoneNumber,
                    OpensAt = clinic.OpensAt,
                    ClosesAt = clinic.ClosesAt,
                    WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    ImagePath = clinic.ImagePath,
                });
            }

            return View(clinicVMs);
        }

        [HttpGet]
        public IActionResult AddClinic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddClinic(AddClinicVM addClinicVM)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var userId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!ModelState.IsValid)
                return View(addClinicVM);




            if (!StaticDetails.allowedImageExtensions.Contains(Path.GetExtension(addClinicVM.Image.FileName)))
            {
                TempData["Error"] = "Invalid image type. Only .jpg, .jpeg, and .png files are allowed.";
                return View(addClinicVM);
            }

            if (addClinicVM.OpensAt > addClinicVM.ClosesAt)
            {
                TempData["Error"] = "Invalid Working Times";
                return View(addClinicVM);
            }

            Clinic clinic = new Clinic()
            {
                Name = addClinicVM.Name,
                Location = addClinicVM.Location,
                PhoneNumber = addClinicVM.PhoneNumber,
                OpensAt = addClinicVM.OpensAt,
                ClosesAt = addClinicVM.ClosesAt,
                WorkingDaysList = addClinicVM.WorkingDaysList,
                DoctorId = userId
            };

            _context.Clinics.Add(clinic);
            _context.SaveChanges();

            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Clinics", clinic.Id.ToString());

            Directory.CreateDirectory(folderPath);

            clinic.ImagePath = _fileService.SaveFile(addClinicVM.Image, folderPath, @"\Images\Clinics\" + clinic.Id.ToString() + "\\");
           


            _context.Clinics.Update(clinic);
            _context.SaveChanges();

            TempData["Success"] = "Clinic is added successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult EditClinic(int Id)
        {
            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == Id);

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic";
                return RedirectToAction(nameof(Index));
            }

            EditClinicVM editClinicVM = new EditClinicVM()
            {
                Id = Id,
                Name = clinic.Name,
                OpensAt = clinic.OpensAt,
                ClosesAt = clinic.ClosesAt,
                Location = clinic.Location,
                PhoneNumber = clinic.PhoneNumber,
                WorkingDaysList = clinic.WorkingDaysList,
                ImagePath = clinic.ImagePath
            };


            return View(editClinicVM);
        }

        [HttpPost]
        public IActionResult EditClinic(EditClinicVM editClinicVM)
        {
            if (!ModelState.IsValid)
                return View(editClinicVM);

            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == editClinicVM.Id);

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic";
                return RedirectToAction(nameof(Index));
            }

            if(editClinicVM.OpensAt >  clinic.OpensAt)
            {
                TempData["Error"] = "Invalid Working Times";
                return View(editClinicVM);
            }

            clinic.Name = editClinicVM.Name;
            clinic.Location = editClinicVM.Location;
            clinic.ClosesAt = editClinicVM.ClosesAt;
            clinic.OpensAt = editClinicVM.OpensAt;
            clinic.WorkingDaysList = editClinicVM.WorkingDaysList;
            clinic.PhoneNumber = editClinicVM.PhoneNumber;

            if (editClinicVM.Image != null)
            {
                if (!StaticDetails.allowedImageExtensions.Contains(Path.GetExtension(editClinicVM.Image.FileName)))
                {
                    TempData["Error"] = "Invalid image type. Only .jpg, .jpeg, and .png files are allowed.";
                    return View(editClinicVM);
                }

                string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Clinics", clinic.Id.ToString());
                clinic.ImagePath = _fileService.UpdateFile(editClinicVM.Image, clinic.ImagePath, folderPath, @"\Images\Clinics\" + clinic.Id.ToString() + "\\");
            }

            _context.Clinics.Update(clinic);
            _context.SaveChanges();

            TempData["Success"] = "Clinic is updated successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
