using ManagementDoctorSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementDoctorSystem.DataAccess
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Pationt> pationts { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          => optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=ManagementDoctorSystem; Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");


    }
}
