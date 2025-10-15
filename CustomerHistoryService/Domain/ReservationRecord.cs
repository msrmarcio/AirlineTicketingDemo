namespace CustomerHistoryService.Domain
{
    public class ReservationRecord
    {
        public Guid Id { get; set; }

        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
