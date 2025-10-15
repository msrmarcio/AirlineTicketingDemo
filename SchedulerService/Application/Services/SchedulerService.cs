using Contracts;
using MassTransit;
using SchedulerService.Application.Interfaces;
using SchedulerService.Domain.Entities;

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

        public async Task SchedulePaymentTimeoutAsync(Guid reservationId, string customerName, string customerEmail)
        {
            try
            {
                var amount = new Random().Next(100, 500); // simulação de valor

                var scheduledReservation = new ScheduledReservation
                {
                    Id = Guid.NewGuid(),
                    ReservationId = reservationId,
                    CustomerName = customerName,
                    CustomerEmail = customerEmail,
                    Amount = amount,
                    CreatedAt = DateTime.UtcNow,
                    TimeoutScheduledFor = DateTime.UtcNow.AddSeconds(30),
                    Status = "Scheduled"
                };

                await _schedulerRepository.AddAsync(scheduledReservation);

                _logger.LogInformation("Agendamento registrado para ReservationId: {ReservationId}", reservationId);

                // Simula espera até o timeout
                await Task.Delay(TimeSpan.FromSeconds(30));

                // Atualiza status e data de execução
                scheduledReservation.Status = "Executed";
                scheduledReservation.ExecutedAt = DateTime.UtcNow;
                await _schedulerRepository.UpdateAsync(scheduledReservation);

                await _publishEndpoint.Publish<IPaymentTimeout>(new PaymentTimeout
                {
                    ReservationId = reservationId,
                    CustomerEmail = customerEmail,
                    Amount = amount
                });

                _logger.LogInformation("PaymentTimeout publicado para ReservationId: {ReservationId}", reservationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao agendar PaymentTimeout para ReservationId: {ReservationId}", reservationId);
                throw;
            }
        }

    }
}
