using Contracts;
using CustomerHistoryService.Application.Interface;
using CustomerHistoryService.Domain;
using MassTransit;

namespace CustomerHistoryService.Consumers
{
    public class PaymentApprovedConsumer : IConsumer<IPaymentApproved>
    {
        private readonly ICustomerHistoryRepository _repository;

        public PaymentApprovedConsumer(ICustomerHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IPaymentApproved> context)
        {
            var msg = context.Message;

            var history = await _repository.GetByEmailAsync(msg.CustomerEmail) ?? new CustomerHistory
            {
                Id = Guid.NewGuid(),
                CustomerEmail = msg.CustomerEmail
            };

            history.Payments.Add(new PaymentRecord
            {
                Status = "Approved",
                ProcessedAt = msg.ProcessedAt
            });

            await _repository.UpsertAsync(history);
        }
    }

}
