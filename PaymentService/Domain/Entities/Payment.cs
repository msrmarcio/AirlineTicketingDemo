namespace PaymentService.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending"; // Approved, Rejected
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }

}
