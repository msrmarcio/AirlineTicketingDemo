using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.Entities;

namespace ReservationService.Infrastructure.Persistence
{
    public class ReservationDbContext : DbContext
    {
        /// <summary>
        /// construtor
        /// </summary>
        /// <param name="options">permite configurar o banco via Program.cs</param>
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// representa a tabela Reservations
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// define regras de mapeamento (chave primária, campos obrigatórios, tamanhos)
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(30);
            });
        }
    }
}
