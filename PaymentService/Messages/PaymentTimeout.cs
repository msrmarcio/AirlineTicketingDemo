namespace PaymentService.Messages;

/// <summary>
/// Essa mensagem deve ser publicada pelo SchedulerService 
/// ou outro serviço que controla o tempo limite de pagamento.
/// </summary>
/// <param name="ReservationId"></param>
/// <param name="CustomerEmail"></param>
/// <param name="Amount"></param>
public record PaymentTimeout(Guid ReservationId, string CustomerEmail, decimal Amount);