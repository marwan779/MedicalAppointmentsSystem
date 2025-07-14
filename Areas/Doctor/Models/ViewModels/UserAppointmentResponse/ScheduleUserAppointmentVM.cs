using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels.UserAppointmentResponse
{
    public class ScheduleUserAppointmentVM
    {
        [ValidateNever]
        public int Id { get; set; }
        [ValidateNever]
        public string DoctorId { get; set; } = string.Empty;
        [ValidateNever]
        public string UserId { get; set; } = string.Empty;
        [ValidateNever]
        public int ClinicId { get; set; }
        [Required]
        public TimeSpan AppointmentStartTime { get; set; }
        [Required]
        public DateOnly AppointmentDate { get; set; }
        [Required]
        public string AppointmentDay { get; set; } = string.Empty;
    }
}
