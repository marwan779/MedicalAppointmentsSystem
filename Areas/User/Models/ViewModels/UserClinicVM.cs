using MedicalAppointmentsSystem.Models;

namespace MedicalAppointmentsSystem.Areas.User.Models.ViewModels
{
    public class UserClinicVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public TimeSpan OpensAt { get; set; }
        public TimeSpan ClosesAt { get; set; }
        public List<string> WorkingDaysList { get; set; } = new List<string>();
        public string ImagePath { get; set; } = string.Empty;
        public string DoctorId { get; set; } = string.Empty;
        public string DoctorSpecialization { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
    }
}
