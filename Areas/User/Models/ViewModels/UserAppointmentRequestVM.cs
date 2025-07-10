using System.ComponentModel.DataAnnotations;
using MedicalAppointmentsSystem.Models;

namespace MedicalAppointmentsSystem.Areas.User.Models.ViewModels
{
    public class UserAppointmentRequestVM
    {
        public string ClinicId { get; set; } = string.Empty;
        public string DoctorId { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public IFormFile Document { get; set; }
        public string RequestStatus { get; set; } = StaticDetails.RequestUnderReview;
        [Required]
        public string ComplainsAbout { get; set; } = string.Empty;
        public string ChronicDiseases { get; set; } = string.Empty;
    }
}
