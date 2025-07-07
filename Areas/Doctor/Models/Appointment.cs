namespace MedicalAppointmentsSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string DoctorId { get; set; } = string.Empty;
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public string Day { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; } = false;
    }
}
