using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace NotificationService.Infrastructure.Persistence
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReservationId).IsRequired();
                entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(300);
                entity.Property(e => e.SentAt).IsRequired();
            });
        }
    }
}
