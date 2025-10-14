using MassTransit;
using PaymentService.Application.Interfaces;
using PaymentService.Messages;

namespace PaymentService.Consumers
{
    public class PaymentTimeoutConsumer : IConsumer<PaymentTimeout>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentTimeoutConsumer> _logger;

        public PaymentTimeoutConsumer(IPaymentService paymentService, ILogger<PaymentTimeoutConsumer> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentTimeout> context)
        {
            var msg = context.Message;

            _logger.LogInformation("Timeout recebido: ReservationId={ReservationId}, Email={Email}, Amount={Amount}",
                msg.ReservationId, msg.CustomerEmail, msg.Amount);

            await _paymentService.ProcessPaymentAsync(msg.ReservationId, msg.CustomerEmail, msg.Amount);
        }
    }
}
