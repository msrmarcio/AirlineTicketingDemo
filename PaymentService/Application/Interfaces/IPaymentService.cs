using PaymentService.Domain.Entities;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Guid reservationId, string customerEmail, decimal amount);
        Task<Payment?> GetPaymentByReservationIdAsync(Guid reservationId);

    }
}
