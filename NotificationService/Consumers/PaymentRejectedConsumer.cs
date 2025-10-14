using MassTransit;
using NATS.Client.Service;
using NotificationService.Application.Interfaces;
using NotificationService.Messages;
using System;

namespace NotificationService.Consumers
{
    /// <summary>
    /// Escuta eventos do RabbitMQ.
    /// Chama o serviço de notificação com a mensagem apropriada.
    /// </summary>
    public class PaymentRejectedConsumer : IConsumer<PaymentRejected>
    {
        private readonly INotificationService _notificationService;

        public PaymentRejectedConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<PaymentRejected> context)
        {
            var msg = context.Message;
            await _notificationService.SendNotificationAsync(
                msg.ReservationId,
                msg.CustomerEmail,
                "PaymentRejected",
                "Sent",
                $"Pagamento rejeitado no valor de R$ {msg.Amount:F2} em {msg.Timestamp:dd/MM/yyyy HH:mm}");

        }
    }
}
