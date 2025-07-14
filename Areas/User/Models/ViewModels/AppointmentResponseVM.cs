namespace MedicalAppointmentsSystem.Areas.User.Models.ViewModels
{
    public class AppointmentResponseVM
    {
        public string Day { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
