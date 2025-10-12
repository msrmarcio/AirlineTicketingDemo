using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;


namespace PaymentService.Infrastructure.Persistence
{
    public class PaymentDbContext : DbContext
    {
        /// <summary>
        /// Construtor que permite configurar o banco via Program.cs
        /// </summary>
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments => Set<Payment>();

        /// <summary>
        /// Define regras de mapeamento (chave primária, campos obrigatórios, tamanhos)
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReservationId).IsRequired();
                entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(10,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(30);
                entity.Property(e => e.ProcessedAt).IsRequired();
            });
        }
    }
}
