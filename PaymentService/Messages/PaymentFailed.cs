namespace PaymentService.Messages;

public record PaymentFailed(Guid ReservationId, string Reason);
