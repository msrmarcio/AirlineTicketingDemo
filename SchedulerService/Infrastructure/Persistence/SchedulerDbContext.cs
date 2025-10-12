using Microsoft.EntityFrameworkCore;
using SchedulerService.Domain.Entities;

namespace SchedulerService.Infrastructure.Persistence
{
    public class SchedulerDbContext : DbContext
    {
        public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options)
            : base(options) { }

        public DbSet<ScheduledReservation> ScheduledReservations => Set<ScheduledReservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduledReservation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(150);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.TimeoutScheduledFor).IsRequired();
            });
        }
    }
}
