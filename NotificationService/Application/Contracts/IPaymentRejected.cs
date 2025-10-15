namespace Contracts
{
    public interface IPaymentRejected
    {
        Guid ReservationId { get; }
        string CustomerEmail { get; }
        decimal Amount { get; }
        DateTime ProcessedAt { get; }
    }
}
