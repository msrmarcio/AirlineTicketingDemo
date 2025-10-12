using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.Persistence
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _dbContext;

        public PaymentRepository(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Payment payment)
        {
            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Payment?> GetByReservationIdAsync(Guid reservationId)
        {
            return await _dbContext.Payments
                .FirstOrDefaultAsync(p => p.ReservationId == reservationId);
        }
    }
}
