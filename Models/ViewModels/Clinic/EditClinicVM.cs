using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MedicalAppointmentsSystem.Models.ViewModels.Clinic
{
    public class EditClinicVM
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public TimeSpan OpensAt { get; set; }
        [Required]
        public TimeSpan ClosesAt { get; set; }
        [Required]
        public List<string> WorkingDaysList { get; set; } = new List<string>();
        [Required]
        public string ImagePath { get; set; } = string.Empty;
        [ValidateNever]
        public IFormFile Image { get; set; }
    }
}
