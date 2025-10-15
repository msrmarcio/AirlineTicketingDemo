using SchedulerService.Domain.Entities;

namespace SchedulerService.Application.Interfaces
{
    public interface ISchedulerRepository
    {
        Task AddAsync(ScheduledReservation scheduledReservation);
        Task UpdateAsync(ScheduledReservation scheduledReservation);
    }
}
