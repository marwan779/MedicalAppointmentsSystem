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
    }
}
