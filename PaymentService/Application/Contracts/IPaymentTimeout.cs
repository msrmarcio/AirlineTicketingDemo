namespace Contracts
{
    public interface IPaymentTimeout
    {
        Guid ReservationId { get; }
        string CustomerEmail { get; }
        decimal Amount { get; }
    }
}
