using CustomerHistoryService.Application.DTOs;

namespace CustomerHistoryService.Application.Interface
{
    public interface ICustomerHistoryReportService
    {
        Task<CustomerHistoryReport?> GetCustomerHistoryReportAsync(string email);
    }

}
