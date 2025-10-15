using SchedulerService.Application.Interfaces;
using SchedulerService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace SchedulerService.Infrastructure.Persistence
{
    public class SchedulerRepository : ISchedulerRepository
    {
        private readonly SchedulerDbContext _dbContext;
        private readonly ILogger<SchedulerRepository> _logger;

        public SchedulerRepository(SchedulerDbContext dbContext, ILogger<SchedulerRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(ScheduledReservation scheduledReservation)
        {
            try
            {
                _dbContext.ScheduledReservations.Add(scheduledReservation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao adicionar agendamento ReservationId: {ReservationId}", scheduledReservation.ReservationId);
                throw;
            }
        }

        public async Task UpdateAsync(ScheduledReservation scheduledReservation)
        {
            try
            {
                _dbContext.ScheduledReservations.Update(scheduledReservation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao atualizar agendamento ReservationId: {ReservationId}", scheduledReservation.ReservationId);
                throw;
            }
        }
    }
}
