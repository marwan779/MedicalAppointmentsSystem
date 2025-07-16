using System.ComponentModel.DataAnnotations;

namespace MedicalAppointmentsSystem.Areas.Doctor.Models
{
    public class DoctorComplaintResponseVM
    {
        public int ComplaintId { get; set; }
        [Required]
        public string Response { get; set; } = string.Empty;

    }
}
