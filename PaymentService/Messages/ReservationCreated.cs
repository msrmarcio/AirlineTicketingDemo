namespace PaymentService.Messages;

public record ReservationCreated(Guid ReservationId, string CustomerName);
