using System.Diagnostics;
using MedicalAppointmentsSystem.Areas.User.Models.ViewModels;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Models.ViewModels.Clinic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentsSystem.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int pageSize = 6, int pageNumber = 1, string searchTerm = "")
        {
            IQueryable<Clinic> clinicsQuery = _context.Clinics.Include(a => a.Doctor).AsQueryable();


            if (!String.IsNullOrEmpty(searchTerm))
            {
                clinicsQuery = clinicsQuery.Where(c => c.Doctor.FullName.Contains(searchTerm)
                || c.Name.Contains(searchTerm)
                || _context.DoctorInformation.FirstOrDefault(d => d.UserId == c.DoctorId).Specialization.Contains(searchTerm)).AsQueryable();
            }

            int totalClinics = _context.Clinics.Count();
            List<Clinic> clinics = clinicsQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();


            
            List<UserClinicVM> userClinicVMs = new List<UserClinicVM>();



            foreach (var clinic in clinics)
            {
                DoctorInformation doctorInformation = _context.DoctorInformation.FirstOrDefault(d => d.UserId == clinic.DoctorId);

                userClinicVMs.Add(new UserClinicVM
                {
                    Id = clinic.Id,
                    Name = clinic.Name,
                    OpensAt = clinic.OpensAt,
                    ClosesAt = clinic.ClosesAt,
                    Location = clinic.Location,
                    PhoneNumber = clinic.PhoneNumber,
                    ImagePath = clinic.ImagePath,
                    WorkingDaysList = clinic.WorkingDays.Split(',').ToList(),
                    DoctorId = clinic.DoctorId,
                    DoctorName = doctorInformation.DoctorName,
                    DoctorSpecialization = doctorInformation.Specialization,
                });
            }

            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalClinics = totalClinics;

            return View(userClinicVMs);
        }

        [Authorize]
        [HttpGet]
        public IActionResult DoctorProfile(string doctorId)
        {
            DoctorInformation? doctorInformation = _context.DoctorInformation
                .FirstOrDefault(d => d.UserId == doctorId);

            if (doctorInformation == null)
            {
                TempData["Error"] = "Now Such Doctor.";
                return RedirectToAction("Index", new {  pageSize = 2,  pageNumber = 1 });
            }

            UserDoctorProfileVM userDoctorProfileVM = new UserDoctorProfileVM()
            {
                DoctorName = doctorInformation.DoctorName,
                LicenseNumber = doctorInformation.LicenseNumber,
                Specialization= doctorInformation.Specialization,
                YearsExperience = doctorInformation.YearsExperience,
                MedicalSchool = doctorInformation.MedicalSchool,
                GraduationYear = doctorInformation.GraduationYear,
                HospitalAffiliation = doctorInformation.HospitalAffiliation,
                Department = doctorInformation.Department,
                Position = doctorInformation.Position,
                Bio = doctorInformation.Bio,
                EmergencyPhone = doctorInformation.EmergencyPhone,
                LicenseImagePath = doctorInformation.LicenseImagePath,
                PhotoPath = doctorInformation.PhotoPath,
            };

            return View(userDoctorProfileVM);
        }
    }
}
