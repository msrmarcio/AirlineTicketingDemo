using CustomerHistoryService.Domain;

namespace CustomerHistoryService.Application.Interface
{
    public interface ICustomerHistoryRepository
    {
        Task<CustomerHistory?> GetByEmailAsync(string email);
        Task UpsertAsync(CustomerHistory history);

    }

}
