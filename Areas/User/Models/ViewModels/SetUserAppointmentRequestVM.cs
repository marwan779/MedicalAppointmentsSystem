using System.ComponentModel.DataAnnotations;
using MedicalAppointmentsSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MedicalAppointmentsSystem.Areas.User.Models.ViewModels
{
    public class SetUserAppointmentRequestVM
    {
        [ValidateNever]
        public int ClinicId { get; set; }
        [ValidateNever]
        public string DoctorId { get; set; } = string.Empty;
        [ValidateNever]
        public IFormFile? Document { get; set; }
        [Required]
        public DateOnly PreferredDate { get; set; }
        [ValidateNever]
        public string RequestStatus { get; set; } = StaticDetails.RequestUnderReview;
        [Required]
        public string ComplainsAbout { get; set; } = string.Empty;
        [ValidateNever]
        public string ChronicDiseases { get; set; } = string.Empty;
    }
}
