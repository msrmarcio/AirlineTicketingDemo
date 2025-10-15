namespace CustomerHistoryService.Application.DTOs
{
    public class CustomerHistoryReport
    {
        public string CustomerEmail { get; set; }
        public List<ReservationDto> Reservations { get; set; }
        public List<PaymentDto> Payments { get; set; }
        public List<NotificationDto> Notifications { get; set; }
    }

    public class ReservationDto
    {
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PaymentDto
    {
        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
    }

    public class NotificationDto
    {
        public string Type { get; set; }
        public DateTime SentAt { get; set; }
    }

}
