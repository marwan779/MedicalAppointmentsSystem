using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalAppointmentsSystem.Models.ViewModels.Doctor
{
    public class AddDoctorInformationVM
    {
        [Required(ErrorMessage = "Medical license number is required")]
        [StringLength(50, ErrorMessage = "License number cannot exceed 50 characters")]
        [Display(Name = "Medical License Number")]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(100, ErrorMessage = "Specialization cannot exceed 100 characters")]
        public string Specialization { get; set; } = string.Empty;
        [ValidateNever]
        public IEnumerable<SelectListItem> SpecializationList { get; set; }

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 60, ErrorMessage = "Experience must be between 0 and 60 years")]
        [Display(Name = "Years of Experience")]
        public int YearsExperience { get; set; }

        [Required(ErrorMessage = "Medical school is required")]
        [StringLength(200, ErrorMessage = "Medical school name cannot exceed 200 characters")]
        [Display(Name = "Medical School")]
        public string MedicalSchool { get; set; } = string.Empty;

        [Required(ErrorMessage = "Graduation year is required")]
        [Range(1900, 2100, ErrorMessage = "Please enter a valid year")]
        [Display(Name = "Graduation Year")]
        public int? GraduationYear { get; set; }

        [Required(ErrorMessage = "Hospital affiliation is required")]
        [StringLength(200, ErrorMessage = "Hospital name cannot exceed 200 characters")]
        [Display(Name = "Hospital/Clinic Name")]
        public string HospitalAffiliation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required")]
        [StringLength(100, ErrorMessage = "Position cannot exceed 100 characters")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bio is required")]
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string Bio { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency phone is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Display(Name = "Emergency Contact Number")]
        public string EmergencyPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "License image is required")]
        [Display(Name = "Medical License Image")]
        public IFormFile LicenseImage { get; set; }

        [Required(ErrorMessage = "ID proof is required")]
        [Display(Name = "Government ID")]
        public IFormFile IdProof { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        [Display(Name = "Professional Photo")]
        public IFormFile Photo { get; set; }
    }
}