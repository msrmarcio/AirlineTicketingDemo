using PaymentService.Domain.Entities;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetByReservationIdAsync(Guid reservationId);

    }
}
