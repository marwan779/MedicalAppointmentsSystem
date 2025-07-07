using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAppointmentsSystem.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public TimeSpan OpensAt { get; set; }
        public TimeSpan ClosesAt { get; set; }
        public string WorkingDays 
        {
            get => string.Join(",", WorkingDaysList);
            set => WorkingDaysList = value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        } 
        [NotMapped]
        public List<string> WorkingDaysList { get; set; } = new();
        public string DoctorId { get; set; } = string.Empty;
        public ApplicationUser Doctor { get; set; }
        public string? ImagePath { get; set; } = string.Empty;
    }
}
