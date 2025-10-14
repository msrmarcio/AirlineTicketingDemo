using NotificationService.Domain.Entities;

namespace NotificationService.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Guid reservationId, string email, string type, string status, string message);
        Task<List<Notification>> GetNotificationsByReservationIdAsync(Guid reservationId);

    }
}
