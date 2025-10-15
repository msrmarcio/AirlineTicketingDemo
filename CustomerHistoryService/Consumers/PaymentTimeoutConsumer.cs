using Contracts;
using CustomerHistoryService.Application.Interface;
using CustomerHistoryService.Domain;
using MassTransit;

namespace CustomerHistoryService.Consumers
{
    public class PaymentTimeoutConsumer : IConsumer<IPaymentTimeout>
    {
        private readonly ICustomerHistoryRepository _repository;

        public PaymentTimeoutConsumer(ICustomerHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IPaymentTimeout> context)
        {
            var msg = context.Message;

            var history = await _repository.GetByEmailAsync(msg.CustomerEmail) ?? new CustomerHistory
            {
                Id = Guid.NewGuid(),
                CustomerEmail = msg.CustomerEmail
            };

            // Atualiza ou adiciona a reserva com valor
            history.Reservations.Add(new ReservationRecord
            {
                ReservationId = msg.ReservationId,
                Amount = msg.Amount,
                CreatedAt = DateTime.UtcNow
            });

            await _repository.UpsertAsync(history);
        }
    }

}
