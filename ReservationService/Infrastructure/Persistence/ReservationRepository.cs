using ReservationService.Application.Interfaces;
using ReservationService.Domain.Entities;

namespace ReservationService.Infrastructure.Persistence
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ReservationDbContext _dbContext;
        private readonly ILogger<ReservationRepository> _logger;

        public ReservationRepository(ReservationDbContext dbContext, ILogger<ReservationRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(Reservation reservation)
        {
            try
            {
                _dbContext.Reservations.Add(reservation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar reserva no banco: {ReservationId}", reservation.Id);
                throw;  
            }
        }
    }
}
