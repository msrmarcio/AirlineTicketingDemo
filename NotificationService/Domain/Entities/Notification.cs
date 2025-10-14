namespace NotificationService.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // PaymentApproved, PaymentRejected
        public string Status { get; set; } = "Sent"; // Sent, Failed
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
