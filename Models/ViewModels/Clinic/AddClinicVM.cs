using System.ComponentModel.DataAnnotations;

namespace MedicalAppointmentsSystem.Models.ViewModels.Clinic
{
    public class AddClinicVM
    {
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
        public IFormFile Image { get; set; }
    }
}
