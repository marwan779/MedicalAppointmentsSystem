namespace MedicalAppointmentsSystem.Models
{
    public class UserComplaint
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string DoctorId { get; set; } = string.Empty;
        public int ClinicId { get; set; }
        public string ComplaintTitle { get; set; } = string.Empty;
        public string ComplaintBody { get; set; } = string.Empty;
        public string? DocumentUrl {  get; set; } = string.Empty;
        public string ComplaintStatus { get; set; }  = StaticDetails.ComplaintUnderReview;
        public bool DoctorResponded { get; set; } = false;
        public string? DoctorReponse { get; set; } = string.Empty;
        public DateTime FiledAt { get; set; } = DateTime.Now;
    }
}
