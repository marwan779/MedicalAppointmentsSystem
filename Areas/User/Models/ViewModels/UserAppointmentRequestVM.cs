using MedicalAppointmentsSystem.Models;

namespace MedicalAppointmentsSystem.Areas.User.Models.ViewModels
{
    public class UserAppointmentRequestVM
    {
        public int Id { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public DateOnly PreferredDate { get; set; }
        public string RequestStatus { get; set; } = StaticDetails.RequestUnderReview;
    }
}
