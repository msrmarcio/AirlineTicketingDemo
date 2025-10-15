using Contracts;

namespace SchedulerService.Domain.Entities
{
    public class PaymentTimeout : IPaymentTimeout
    {
        public Guid ReservationId { get; init; }
        public string CustomerEmail { get; init; }
        public decimal Amount { get; init; }  
    }

}
