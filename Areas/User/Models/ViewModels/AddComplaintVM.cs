using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalAppointmentsSystem.Areas.User.Models.ViewModels
{
    public class AddComplaintVM
    {
        [Required]
        public int ClinicId { get; set; }
        [Required]
        public string ComplaintBody { get; set; } = string.Empty;
        [Required]
        public string ComplaintTitle { get; set; } = string.Empty;
        [ValidateNever]
        public IFormFile? Document { get; set; } 
        
    }
}
