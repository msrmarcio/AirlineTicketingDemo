using MassTransit;
using PaymentService.Application.Interfaces;
using PaymentService.Messages;

namespace PaymentService.Consumers
{
    public class PaymentTimeoutConsumer : IConsumer<PaymentTimeout>
    {
        private readonly IPaymentService _paymentService;

        public PaymentTimeoutConsumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Consume(ConsumeContext<PaymentTimeout> context)
        {
            var message = context.Message;

            await _paymentService.ProcessPaymentAsync(
                message.ReservationId,
                message.Amount,
                message.CustomerEmail);
        }
    }

}
