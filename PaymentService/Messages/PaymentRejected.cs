using Contracts;

namespace PaymentService.Messages;

public class PaymentRejected : IPaymentRejected
{
    public Guid ReservationId { get; init; }
    public string CustomerEmail { get; init; }
    public decimal Amount { get; init; }
    public DateTime ProcessedAt { get; init; }

    public PaymentRejected(Guid reservationId, string customerEmail, decimal amount, DateTime processedAt)
    {
        ReservationId = reservationId;
        CustomerEmail = customerEmail;
        Amount = amount;
        ProcessedAt = processedAt;
    }
}