using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Persistence
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _dbContext;

        public NotificationRepository(NotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Notification notification)
        {
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetByReservationIdAsync(Guid reservationId)
        {
            return await _dbContext.Notifications
                .Where(n => n.ReservationId == reservationId)
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }

    }
}
