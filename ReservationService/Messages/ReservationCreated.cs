namespace ReservationService.Messages;

public record ReservationCreated(Guid ReservationId, string CustomerName, string CustomerEmail);
