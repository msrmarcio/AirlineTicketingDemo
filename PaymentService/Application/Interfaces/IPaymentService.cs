using PaymentService.Domain.Entities;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Guid reservationId, decimal amount, string customerEmail);
        Task<Payment?> GetPaymentByReservationIdAsync(Guid reservationId);

    }
}
