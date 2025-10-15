namespace SchedulerService.Messages
{
    public record ReservationCreated(Guid ReservationId, string CustomerName, string CustomerEmail);

}
