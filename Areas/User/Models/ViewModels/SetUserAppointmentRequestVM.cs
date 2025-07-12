using System.ComponentModel.DataAnnotations;
using MedicalAppointmentsSystem.Models;

namespace MedicalAppointmentsSystem.Areas.User.Models.ViewModels
{
    public class SetUserAppointmentRequestVM
    {
        public int ClinicId { get; set; }
        public string DoctorId { get; set; } = string.Empty;
        public IFormFile Document { get; set; }
        public DateOnly PreferredDate { get; set; }
        public string RequestStatus { get; set; } = StaticDetails.RequestUnderReview;
        [Required]
        public string ComplainsAbout { get; set; } = string.Empty;
        public string ChronicDiseases { get; set; } = string.Empty;
    }
}
