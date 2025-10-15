using Contracts;
using CustomerHistoryService.Application.Interface;
using CustomerHistoryService.Domain;
using MassTransit;

namespace CustomerHistoryService.Consumers
{
    public class ReservationCreatedConsumer : IConsumer<IReservationCreated>
    {
        private readonly ICustomerHistoryRepository _repository;

        public ReservationCreatedConsumer(ICustomerHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IReservationCreated> context)
        {
            var msg = context.Message;

            var history = await _repository.GetByEmailAsync(msg.CustomerEmail) ?? new CustomerHistory
            {
                Id = Guid.NewGuid(),
                CustomerEmail = msg.CustomerEmail
            };

            history.Reservations.Add(new ReservationRecord
            {
                ReservationId = msg.ReservationId,
                CreatedAt = DateTime.UtcNow
            });


            await _repository.UpsertAsync(history);
        }
    }

}
