namespace NotificationService.Application.DTOs
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
