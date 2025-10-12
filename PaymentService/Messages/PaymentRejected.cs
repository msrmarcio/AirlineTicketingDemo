namespace PaymentService.Messages;

public record PaymentRejected(Guid ReservationId, string CustomerEmail, decimal Amount, DateTime Timestamp);