namespace PaymentService.Messages;

public record PaymentProcessed(Guid ReservationId, bool success);
