using Contracts;
using CustomerHistoryService.Application.Interface;
using CustomerHistoryService.Domain;
using MassTransit;

namespace CustomerHistoryService.Consumers
{
    public class PaymentRejectedConsumer : IConsumer<IPaymentRejected>
    {
        private readonly ICustomerHistoryRepository _repository;

        public PaymentRejectedConsumer(ICustomerHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IPaymentRejected> context)
        {
            var msg = context.Message;

            var history = await _repository.GetByEmailAsync(msg.CustomerEmail) ?? new CustomerHistory
            {
                Id = Guid.NewGuid(),
                CustomerEmail = msg.CustomerEmail
            };

            history.Payments.Add(new PaymentRecord
            {
                Status = "Rejected",
                ProcessedAt = msg.ProcessedAt
            });

            await _repository.UpsertAsync(history);
        }
    }

}
