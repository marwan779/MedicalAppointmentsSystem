using System.Security.Claims;
using MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels.Appointments;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Models.ViewModels.Appointments;
using MedicalAppointmentsSystem.Models.ViewModels.Clinic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Areas.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = StaticDetails.DoctorRole)]
    public class AvailabilityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvailabilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int clinicId)
        {
            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic";
                return RedirectToAction("Index", "ClinicController");
            }

            List<DoctorAvailability> doctorAvailabilities = _context.DoctorAvailabilities.Where(a => a.ClinicId == clinicId).ToList();
            List<DoctorAvailabilityVM> doctorAvailabilityVMs = new List<DoctorAvailabilityVM>();

            foreach (DoctorAvailability doctorAvailability in doctorAvailabilities)
            {
                doctorAvailabilityVMs.Add
                    (
                        new DoctorAvailabilityVM()
                        {
                            Id = doctorAvailability.Id,
                            Day = doctorAvailability.Day,
                            StartTime = doctorAvailability.StartTime,
                            EndTime = doctorAvailability.EndTime,
                            ClinicId = doctorAvailability.ClinicId
                        }
                    );
            }

            ViewBag.ClinicId = clinicId;
            return View(doctorAvailabilityVMs);
        }

        [HttpGet]
        public IActionResult AddAvailability(int clinicId)
        {
            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);
            List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            ViewBag.ClinicId = clinicId;
            ViewBag.WorkingDays = WorkingDaysList;
            return View();
        }

        [HttpPost]
        public IActionResult AddAvailability(AddDoctorAvailabilityVM addDoctorAvailabilityVM, int clinicId)
        {

            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic";
                return RedirectToAction(nameof(Index), new { clinicId });
            }

            if (!ModelState.IsValid)
            {
                List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                ViewBag.WorkingDays = WorkingDaysList;
                ViewBag.ClinicId = clinicId;
                return View(addDoctorAvailabilityVM);
            }

            if (addDoctorAvailabilityVM.StartTime < clinic.OpensAt ||
               addDoctorAvailabilityVM.EndTime > clinic.ClosesAt)
            {
                TempData["Error"] = "The Clinic Is Closed At This Time";
                List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                ViewBag.WorkingDays = WorkingDaysList;
                ViewBag.ClinicId = clinicId;
                return View(addDoctorAvailabilityVM);
            }

            if (addDoctorAvailabilityVM.StartTime >= addDoctorAvailabilityVM.EndTime)
            {
                TempData["Error"] = "Invalid Time Interval";
                List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                ViewBag.WorkingDays = WorkingDaysList;
                ViewBag.ClinicId = clinicId;
                return View(addDoctorAvailabilityVM);
            }

            List<DoctorAvailability> doctorAvailabilities = _context.DoctorAvailabilities.Where(a => a.DoctorId == UserId).ToList();


            foreach (DoctorAvailability globalAvailability in doctorAvailabilities)
            {
                if (globalAvailability.Day == addDoctorAvailabilityVM.Day &&
                    addDoctorAvailabilityVM.StartTime < globalAvailability.EndTime &&
                    addDoctorAvailabilityVM.EndTime > globalAvailability.StartTime)
                {
                    TempData["Error"] = "The selected time interval overlaps with an existing availability," +
                        " which may be in this clinic or another clinic you’re assigned to.\r\nPlease choose a different time.";
                    List<string> WorkingDaysList = clinic.WorkingDays
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
                    ViewBag.WorkingDays = WorkingDaysList;
                    ViewBag.ClinicId = clinicId;
                    return View(addDoctorAvailabilityVM);
                }
            }

            DoctorAvailability doctorAvailability = new DoctorAvailability()
            {
                ClinicId = clinicId,
                Day = addDoctorAvailabilityVM.Day,
                StartTime = addDoctorAvailabilityVM.StartTime,
                EndTime = addDoctorAvailabilityVM.EndTime,
                Clinic = clinic,
                DoctorId = UserId
            };

            _context.DoctorAvailabilities.Add(doctorAvailability);
            _context.SaveChanges();

            TempData["Success"] = "Your availability is added successfully";
            return RedirectToAction(nameof(Index), new { clinicId });
        }

        [HttpGet]
        public IActionResult EditAvailability(int availabilityId, int clinicId)
        {
            DoctorAvailability? doctorAvailability = _context.DoctorAvailabilities.FirstOrDefault(a => a.Id == availabilityId);
            //int clinicId = ViewBag.clinicId;

            if (doctorAvailability == null)
            {
                TempData["Error"] = "No such an availability.";
                return RedirectToAction(nameof(Index), new { clinicId });
            }



            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic.";
                return RedirectToAction(nameof(Index), new { clinicId });
            }

            EditDoctorAvailabilityVM editDoctorAvailabilityVM = new EditDoctorAvailabilityVM()
            {
                ClinicId = clinicId,
                Day = doctorAvailability.Day,
                StartTime = doctorAvailability.StartTime,
                EndTime = doctorAvailability.EndTime,
                Id = doctorAvailability.Id,
            };

            List<string> WorkingDaysList = clinic.WorkingDays
                       .Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .ToList();
            ViewBag.WorkingDays = WorkingDaysList;
            ViewBag.ClinicId = clinicId;

            return View(editDoctorAvailabilityVM);

        }

        [HttpPost]
        public IActionResult EditAvailability(EditDoctorAvailabilityVM editDoctorAvailabilityVM, int clinicId)
        {

            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic";
                return RedirectToAction(nameof(Index), new { clinicId });
            }

            if (!ModelState.IsValid)
            {
                List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                ViewBag.WorkingDays = WorkingDaysList;
                ViewBag.ClinicId = clinicId;
                return View(editDoctorAvailabilityVM);
            }

            if (editDoctorAvailabilityVM.StartTime < clinic.OpensAt ||
               editDoctorAvailabilityVM.EndTime > clinic.ClosesAt)
            {
                TempData["Error"] = "The Clinic Is Closed At This Time";
                List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                ViewBag.WorkingDays = WorkingDaysList;
                ViewBag.ClinicId = clinicId;
                return View(editDoctorAvailabilityVM);
            }

            if (editDoctorAvailabilityVM.StartTime >= editDoctorAvailabilityVM.EndTime)
            {
                TempData["Error"] = "Invalid Time Interval";
                List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                ViewBag.WorkingDays = WorkingDaysList;
                ViewBag.ClinicId = clinicId;
                return View(editDoctorAvailabilityVM);
            }

            List<DoctorAvailability> doctorAvailabilities = _context.DoctorAvailabilities.Where(a => a.DoctorId == UserId).ToList();


            // get the same time slot to remove it from the list before validation 
            DoctorAvailability? doctorAvailability = doctorAvailabilities
                .FirstOrDefault(a => a.Id == editDoctorAvailabilityVM.Id);

            doctorAvailabilities.Remove(doctorAvailability);

            foreach (DoctorAvailability globalAvailability in doctorAvailabilities)
            {
                if (globalAvailability.Day == editDoctorAvailabilityVM.Day &&
                    editDoctorAvailabilityVM.StartTime < globalAvailability.EndTime &&
                    editDoctorAvailabilityVM.EndTime > globalAvailability.StartTime)
                {
                    TempData["Error"] = "The selected time interval overlaps with an existing availability," +
                        " which may be in this clinic or another clinic you’re assigned to.\r\nPlease choose a different time.";
                    List<string> WorkingDaysList = clinic.WorkingDays
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
                    ViewBag.WorkingDays = WorkingDaysList;
                    ViewBag.ClinicId = clinicId;
                    return View(editDoctorAvailabilityVM);
                }
            }


            doctorAvailability.ClinicId = clinicId;
            doctorAvailability.Day = editDoctorAvailabilityVM.Day;
            doctorAvailability.StartTime = editDoctorAvailabilityVM.StartTime;
            doctorAvailability.EndTime = editDoctorAvailabilityVM.EndTime;
            doctorAvailability.Clinic = clinic;
            doctorAvailability.DoctorId = UserId;

            _context.DoctorAvailabilities.Update(doctorAvailability);
            _context.SaveChanges();

            TempData["Success"] = "Your availability is updated successfully";
            return RedirectToAction(nameof(Index), new { clinicId });

        }

        [HttpGet]
        public IActionResult DeleteAvailability(int availabilityId, int clinicId)
        {
            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic";
                return RedirectToAction(nameof(Index), new { clinicId });
            }

            DoctorAvailability? doctorAvailability = _context.DoctorAvailabilities.FirstOrDefault(a => a.Id == availabilityId);

            if (doctorAvailability == null)
            {
                TempData["Error"] = "No such time interval";
                return RedirectToAction(nameof(Index), new { clinicId });
            }

            _context.DoctorAvailabilities.Remove(doctorAvailability);
            _context.SaveChanges();

            TempData["Success"] = "Your availability is removed successfully";
            return RedirectToAction(nameof(Index), new { clinicId });
        }

    }
}
