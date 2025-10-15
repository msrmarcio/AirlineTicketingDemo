namespace Contracts
{
    public interface IReservationCreated
    {
        Guid ReservationId { get; }
        string CustomerName { get; }
        string CustomerEmail { get; }
    }
}
