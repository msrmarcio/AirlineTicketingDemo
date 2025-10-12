namespace PaymentService.Messages;

public record PaymentApproved(Guid ReservationId, string CustomerEmail, decimal Amount, DateTime Timestamp);