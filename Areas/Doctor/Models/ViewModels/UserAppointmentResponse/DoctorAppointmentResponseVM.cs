namespace MedicalAppointmentsSystem.Areas.Doctor.Models.ViewModels.UserAppointmentResponse
{
    public class DoctorAppointmentResponseVM
    {
        public string Day { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
