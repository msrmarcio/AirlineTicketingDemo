namespace Contracts
{
    public interface INotificationSent
    {
        Guid ReservationId { get; }
        string CustomerEmail { get; }
        string Type { get; }
        string Status { get; }
        string Message { get; }
        DateTime SentAt { get; }
    }

}
