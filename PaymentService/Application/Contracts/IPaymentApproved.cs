namespace Contracts
{
    public interface IPaymentApproved
    {
        Guid ReservationId { get; }
        string CustomerEmail { get; }
        decimal Amount { get; }
        DateTime ProcessedAt { get; }
    }
}
