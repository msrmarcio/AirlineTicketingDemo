namespace SchedulerService.Messages
{
    public record PaymentTimeout(Guid ReservationId, string CustomerEmail, decimal Amount);

}
