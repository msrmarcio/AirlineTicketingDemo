namespace PaymentService.Application.DTOs
{
    public class ProcessPaymentRequestDto
    {
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public bool Success { get; set; }
    }
}
