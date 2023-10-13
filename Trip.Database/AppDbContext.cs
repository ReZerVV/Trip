using Microsoft.EntityFrameworkCore;
using Trip.Domain;

namespace Trip.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Domain.Trip> Trips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasMany(user => user.Notifications)
                .WithOne(notification => notification.Reciver)
                .HasForeignKey(notification => notification.ReciverId);
        }
    }
}
