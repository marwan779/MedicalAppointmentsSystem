using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MedicalAppointmentsSystem.Models.ViewModels.Appointments
{
    public class AddDoctorAvailabilityVM
    {
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
