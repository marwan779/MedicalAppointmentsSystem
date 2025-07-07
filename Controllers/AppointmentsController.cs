using System.Security.Claims;
using MedicalAppointmentsSystem.Data;
using MedicalAppointmentsSystem.Models;
using MedicalAppointmentsSystem.Models.ViewModels.Appointments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentsSystem.Controllers
{
    [Authorize(Roles = StaticDetails.DoctorRole)]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
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

            List<Appointment> appointments = _context.Appointments.Where(a => a.ClinicId == clinicId).ToList();
            List<AppointmentVM> appointmentVMs = new List<AppointmentVM>();

            foreach (Appointment appointment in appointments)
            {
                appointmentVMs.Add
                    (
                        new AppointmentVM()
                        {
                            Id = appointment.Id,
                            Day = appointment.Day,
                            StartTime = appointment.StartTime,
                            EndTime = appointment.EndTime,
                            IsBooked = appointment.IsBooked,
                            ClinicId = appointment.ClinicId,
                        }
                    );
            }

            ViewBag.ClinicId = clinicId;
            return View(appointmentVMs);
        }

        [HttpGet]
        public IActionResult AddAppointment(int clinicId)
        {
            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);
            List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            ViewBag.ClinicId = clinicId;
            ViewBag.WorkingDays = WorkingDaysList;
            return View();
        }

        [HttpPost]
        public IActionResult AddAppointment(AddAppointmentVM addAppointmentVM, int clinicId)
        {

            Clinic? clinic = _context.Clinics.FirstOrDefault(c => c.Id == clinicId);

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsidentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (clinic == null)
            {
                TempData["Error"] = "No such clinic";
                return RedirectToAction(nameof(Index), new {clinicId});
            }

            if (!ModelState.IsValid)
            {
                List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                ViewBag.WorkingDays = WorkingDaysList;
                ViewBag.ClinicId = clinicId;
                return View(addAppointmentVM);
            }


            if (addAppointmentVM.StartTime >= addAppointmentVM.EndTime)
            {
                TempData["Error"] = "Invalid appointment";
                List<string> WorkingDaysList = clinic.WorkingDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                ViewBag.WorkingDays = WorkingDaysList;
                ViewBag.ClinicId = clinicId;
                return View(addAppointmentVM);
            }

            List<Appointment> appointments = _context.Appointments.Where(a => a.DoctorId == UserId).ToList();


            foreach (Appointment globalAppointment in appointments)
            {
                if (globalAppointment.Day == addAppointmentVM.Day &&
                    addAppointmentVM.StartTime < globalAppointment.EndTime &&
                    addAppointmentVM.EndTime > globalAppointment.StartTime)
                {
                    TempData["Error"] = "This appointment conflicts with another appointment.";
                    List<string> WorkingDaysList = clinic.WorkingDays
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
                    ViewBag.WorkingDays = WorkingDaysList;
                    ViewBag.ClinicId = clinicId;
                    return View(addAppointmentVM);
                }
            }

            Appointment appointment = new Appointment()
            {
                ClinicId = clinicId,
                Day = addAppointmentVM.Day,
                StartTime = addAppointmentVM.StartTime,
                EndTime = addAppointmentVM.EndTime,
                IsBooked = false,
                Clinic = clinic,
                DoctorId = UserId
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            TempData["Success"] = "Appointment is set successfully";
            return RedirectToAction(nameof(Index), new { clinicId });
        }

    }
}
