using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppCrimeMapContext(DbContextOptions<AppCrimeMapContext> options) : DbContext(options)
    {
        public DbSet<Crime> Crimes { get; set; } = null!;
        public DbSet<CrimeType> CrimeTypes { get; set; } = null!;
        public DbSet<WantedPerson> WantedPersons { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Crime>()
                .HasOne(c => c.WantedPerson)
                .WithMany(p => p.Crimes)
                .HasForeignKey(c => c.WantedPersonId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Crime>()
                .HasOne(c => c.Creator)
                .WithMany(ct => ct.CreateCrimes)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Crime>()
                .HasOne(c => c.Editor)
                .WithMany(ct => ct.EditCrimes)
                .HasForeignKey(c => c.EditorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.HasPostgresExtension("postgis");

            base.OnModelCreating(modelBuilder);
        }
    }
}
