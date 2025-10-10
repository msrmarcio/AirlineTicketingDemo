using PaymentService.Application.Interfaces;
using MassTransit;
using PaymentService.Messages;

namespace PaymentService.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<PaymentService> _logger;
        public PaymentService(IPublishEndpoint publishEndpoint, ILogger<PaymentService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<bool> ProcessPaymentAsync(Guid reservationId, decimal amount)
        {
            // simulação: 70% de chance de sucesso no pagamento
            var success = new Random().Next(0, 10) <= 7;

            if (success)
            {
                await _publishEndpoint.Publish(new PaymentProcessed(reservationId, true));
                _logger.LogInformation("Payment processed successfully for ReservationId: {ReservationId}, Amount: {Amount}", reservationId, amount);

                return true;
            }
            else
            {
                await _publishEndpoint.Publish(new PaymentFailed(reservationId, "Payment denied by the simulator"));
                return false;
            }
        }
    }
}
