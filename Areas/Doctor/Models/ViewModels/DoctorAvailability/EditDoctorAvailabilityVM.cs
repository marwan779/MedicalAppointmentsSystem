using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels.Appointments
{
    public class EditDoctorAvailabilityVM
    {
        [ValidateNever]
        public int Id { get; set; }
        [ValidateNever]
        public int ClinicId { get; set; }
        [Required]
        public string Day { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
    }
}
