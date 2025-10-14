namespace NotificationService.Messages;

public record PaymentApproved(Guid ReservationId, string CustomerEmail, decimal Amount, DateTime Timestamp);

/*
Escutar eventos como PaymentApproved e PaymentRejected vindos do RabbitMQ
Define os contratos das mensagens que o serviço vai escutar.
Essas mensagens são publicadas pelo PaymentService.
*/