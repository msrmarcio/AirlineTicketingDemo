using MassTransit;
using SchedulerService.Application.Interfaces;
using SchedulerService.Domain.Entities;
using SchedulerService.Messages;

namespace SchedulerService.Application.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISchedulerRepository _schedulerRepository;
        private readonly ILogger<SchedulerService> _logger;

        public SchedulerService(
            IPublishEndpoint publishEndpoint,
            ISchedulerRepository schedulerRepository,
            ILogger<SchedulerService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _schedulerRepository = schedulerRepository;
            _logger = logger;
        }

        public async Task SchedulePaymentTimeoutAsync(Guid reservationId, string customerEmail)
        {
            var scheduledReservation = new ScheduledReservation
            {
                Id = Guid.NewGuid(),
                ReservationId = reservationId,
                CustomerEmail = customerEmail,
                ScheduledAt = DateTime.UtcNow,
                TimeoutAt = DateTime.UtcNow.AddSeconds(30) // simulação de 30 segundos
            };

            await _schedulerRepository.AddAsync(scheduledReservation);

            _logger.LogInformation("🗓️ Agendamento registrado para ReservationId: {ReservationId}", reservationId);

            // Simula espera até o timeout
            await Task.Delay(TimeSpan.FromSeconds(30));

            var amount = new Random().Next(100, 500); // simulação

            await _publishEndpoint.Publish(new PaymentTimeout(reservationId, customerEmail, amount));

            _logger.LogInformation("⏰ PaymentTimeout publicado para ReservationId: {ReservationId}", reservationId);
        }
    }
}
