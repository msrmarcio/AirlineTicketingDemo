using MassTransit;
using PaymentService.Application.Interfaces;
using PaymentService.Messages;

namespace PaymentService.Consumers
{
    public class ReservationCreatedConsumer : IConsumer<ReservationCreated>
    {
        private readonly ILogger<ReservationCreatedConsumer> _logger;
        private readonly IPaymentService _pamentService;

        public ReservationCreatedConsumer(ILogger<ReservationCreatedConsumer> logger, IPaymentService pamentService)
        {
            _logger = logger;
            _pamentService = pamentService;
        }

        public async Task Consume(ConsumeContext<ReservationCreated> context)
        {
            var message = context.Message;
            
            await _pamentService.ProcessPaymentAsync(message.ReservationId, 1000m); // valor fixo para simulação
        }
    }
}
