using CustomerHistoryService.Application.DTOs;
using CustomerHistoryService.Application.Interface;
using CustomerHistoryService.Infrastructure.Persistence;

namespace CustomerHistoryService.Application.Services
{
    public class CustomerHistoryReportService : ICustomerHistoryReportService
    {
        private readonly ICustomerHistoryRepository _repository;

        public CustomerHistoryReportService(ICustomerHistoryRepository repository)
        {
            _repository = repository;
        }
         
        public async Task<CustomerHistoryReport?> GetCustomerHistoryReportAsync(string email)
        {
            var history = await _repository.GetByEmailAsync(email);
            if (history == null)
                return null;

            return new CustomerHistoryReport
            {
                CustomerEmail = history.CustomerEmail,
                Reservations = history.Reservations.Select(r => new ReservationDto
                {
                    ReservationId = r.ReservationId,
                    Amount = r.Amount,
                    CreatedAt = r.CreatedAt
                }).ToList(),
                Payments = history.Payments.Select(p => new PaymentDto
                {
                    Status = p.Status,
                    ProcessedAt = p.ProcessedAt
                }).ToList(),
                Notifications = history.Notifications.Select(n => new NotificationDto
                {
                    Type = n.Type,
                    SentAt = n.SentAt
                }).ToList()
            };
        }
    }

}
