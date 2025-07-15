using MedicalAppointmentsSystem.Areas.Doctor.Models;
using MedicalAppointmentsSystem.Areas.User.Models;
using MedicalAppointmentsSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentsSystem.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options) 
        {
            
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<DoctorInformation> DoctorInformation { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<UserAppointmentRequest> userAppointmentRequests { get; set; }
        public DbSet<UserAppointmentResponse> UserAppointmentResponses { get; set; }
        public DbSet<UserComplaint> UserComplaints { get; set; }
    }
}
