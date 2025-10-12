namespace SchedulerService.Domain.Entities
{
    public class ScheduledReservation
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime TimeoutScheduledFor { get; set; }
    }
}
