using System.ComponentModel.DataAnnotations;

namespace MedicalAppointmentsSystem.Models
{
    public class DoctorInformation
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Range(0, 60)]
        public int YearsExperience { get; set; }

        [StringLength(200)]
        public string MedicalSchool { get; set; } = string.Empty;

        public int? GraduationYear { get; set; }

        [Required]
        [StringLength(200)]
        public string HospitalAffiliation { get; set; } = string.Empty;

        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [StringLength(500)]
        public string Bio { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string EmergencyPhone { get; set; } = string.Empty;

        [StringLength(255)]
        public string LicenseImagePath { get; set; } = string.Empty;

        [StringLength(255)]
        public string IdProofPath { get; set; } = string.Empty;

        [StringLength(255)]
        public string PhotoPath { get; set; } = string.Empty;

        public bool IsVerified { get; set; } = true;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    }
}
