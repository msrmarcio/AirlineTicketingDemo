using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.Entities;

namespace ReservationService.Infrastructure.Persistence
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options)
            : base(options)
        {
        }

        // Representa a tabela Reservations no banco de dados
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>(entity =>
            {
                // Chave primária
                entity.HasKey(e => e.Id);

                // Campos obrigatórios e restrições de tamanho
                entity.Property(e => e.CustomerName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.CustomerEmail)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CreatedAt)
                      .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
