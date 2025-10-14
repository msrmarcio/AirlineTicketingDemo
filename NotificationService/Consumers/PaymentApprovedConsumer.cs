using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Messages;

namespace NotificationService.Consumers
{
    /// <summary>
    /// Escuta eventos do RabbitMQ.
    /// Chama o serviço de notificação com a mensagem apropriada.
    /// </summary>
    public class PaymentApprovedConsumer : IConsumer<PaymentApproved>
    {
        private readonly INotificationService _notificationService;

        public PaymentApprovedConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<PaymentApproved> context)
        {
            var msg = context.Message;
            await _notificationService.SendNotificationAsync(
                msg.ReservationId,
                msg.CustomerEmail,
                "PaymentApproved",
                "Sent",
                $"Pagamento aprovado no valor de R$ {msg.Amount:F2} em {msg.Timestamp:dd/MM/yyyy HH:mm}");

        }
    }
}
