namespace PaymentService.Application.DTOs
{
    public class ProcessPaymentRequestDto
    {
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
    }
}
