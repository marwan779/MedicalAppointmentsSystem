namespace MedicalAppointmentsSystem.Models.ViewModels.Appointments
{
    public class DoctorAvailabilityVM
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public string Day { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
