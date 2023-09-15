using Microsoft.EntityFrameworkCore;

namespace Backend.DataModels
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Driver>(e =>
            {
               
                e.HasKey(x => x.Id);

                e.Property(x => x.FirstName)
                .IsRequired(true)
                .HasMaxLength(50);

                e.Property(x => x.LastName)
                .IsRequired(true)
                .HasMaxLength(50);

                e.Property(x => x.Email)
                .IsRequired(true)
                .HasMaxLength(100);

                e.Property(x => x.PhoneNumber)
                .IsRequired(true)
                .HasMaxLength(13);
            });
        }
    }
}
