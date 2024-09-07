using Microsoft.EntityFrameworkCore;
using TestTask.Domain.Entities;

namespace TestTask.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Uchastok> Uchastoks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UchastokDbConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationDbConfiguration());
            modelBuilder.ApplyConfiguration(new CabinetDbConfiguration());
        }
    }
}
