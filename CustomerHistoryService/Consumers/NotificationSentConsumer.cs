using Contracts;
using CustomerHistoryService.Application.Interface;
using CustomerHistoryService.Domain;
using MassTransit;

namespace CustomerHistoryService.Consumers
{
    public class NotificationSentConsumer : IConsumer<INotificationSent>
    {
        private readonly ICustomerHistoryRepository _repository;

        public NotificationSentConsumer(ICustomerHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<INotificationSent> context)
        {
            var msg = context.Message;

            var history = await _repository.GetByEmailAsync(msg.CustomerEmail) ?? new CustomerHistory
            {
                Id = Guid.NewGuid(),
                CustomerEmail = msg.CustomerEmail
            };

            history.Notifications.Add(new NotificationRecord
            {
                Type = msg.Type,
                SentAt = msg.SentAt
            });

            await _repository.UpsertAsync(history);
        }
    }

}
