using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.Application.Services
{
    public class NotificationServices : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationServices> _logger;

        public NotificationServices(INotificationRepository notificationRepository, ILogger<NotificationServices> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task SendNotificationAsync(Guid reservationId, string email, string type, string status, string message)
        {
            // Persiste a notificação no banco
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                ReservationId = reservationId,
                CustomerEmail = email,
                Type = type,
                Status = status,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);



            _logger.LogInformation("Notificação [{Type}] enviada para {Email} com status {Status}: {Message}", type, email, status, message);

            // Aqui você pode integrar com um serviço de envio de e-mail
            // Exemplo:
            // await _emailSender.SendAsync(email, $"Notificação: {type}", message);
        }

        public async Task<List<Notification>> GetNotificationsByReservationIdAsync(Guid reservationId)
        {
            return await _notificationRepository.GetByReservationIdAsync(reservationId);
        }
    }
}
