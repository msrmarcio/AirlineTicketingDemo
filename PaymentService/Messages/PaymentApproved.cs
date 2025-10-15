using Contracts;

namespace PaymentService.Messages;

public class PaymentApproved : IPaymentApproved
{
    public Guid ReservationId { get; init; }
    public string CustomerEmail { get; init; }
    public decimal Amount { get; init; }
    public DateTime ProcessedAt { get; init; }

    public PaymentApproved(Guid reservationId, string customerEmail, decimal amount, DateTime processedAt)
    {
        ReservationId = reservationId;
        CustomerEmail = customerEmail;
        Amount = amount;
        ProcessedAt = processedAt;
    }
}