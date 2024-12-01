using HealthBook.Model;
using Microsoft.EntityFrameworkCore;

namespace HealthBook.Data
{
    public class HealthBookContext : DbContext
    {
        public HealthBookContext(DbContextOptions<HealthBookContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<HealthProfessional> HealthProfessionals { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
    }
}
