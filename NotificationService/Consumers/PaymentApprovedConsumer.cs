using Contracts;
using MassTransit;
using NotificationService.Application.Interfaces;

namespace NotificationService.Consumers
{
    /// <summary>
    /// Escuta eventos do RabbitMQ.
    /// Chama o serviço de notificação com a mensagem apropriada.
    /// </summary>
    public class PaymentApprovedConsumer : IConsumer<IPaymentApproved>
    {
        private readonly INotificationService _notificationService;

        public PaymentApprovedConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<IPaymentApproved> context)
        {
            var msg = context.Message;

            // Persiste a notificação no banco via serviço
            await _notificationService.SendNotificationAsync(
                msg.ReservationId,
                msg.CustomerEmail,
                "PaymentApproved",
                "Sent",
                $"Pagamento aprovado no valor de R$ {msg.Amount:F2} em {msg.ProcessedAt:dd/MM/yyyy HH:mm}");

            // Aqui você pode acionar o envio de e-mail, SMS ou push notification
            // Exemplo: await _emailSender.SendAsync(msg.CustomerEmail, "Pagamento aprovado", corpoDoEmail);
        }
    }
}
