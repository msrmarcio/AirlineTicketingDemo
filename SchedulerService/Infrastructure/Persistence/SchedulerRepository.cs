using SchedulerService.Application.Interfaces;
using SchedulerService.Domain.Entities;

namespace SchedulerService.Infrastructure.Persistence
{
    public class SchedulerRepository : ISchedulerRepository
    {
        private readonly SchedulerDbContext _dbContext;

        public SchedulerRepository(SchedulerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ScheduledReservation scheduledReservation)
        {
            _dbContext.ScheduledReservations.Add(scheduledReservation);
            await _dbContext.SaveChangesAsync();
        }
    }
}
