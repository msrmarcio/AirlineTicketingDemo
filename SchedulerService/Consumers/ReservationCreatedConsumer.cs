using Contracts;
using MassTransit;
using SchedulerService.Application.Interfaces;

namespace SchedulerService.Consumers
{
    public class ReservationCreatedConsumer : IConsumer<IReservationCreated>
    {
        private readonly ISchedulerService _schedulerService;
        private readonly ILogger<ReservationCreatedConsumer> _logger;

        public ReservationCreatedConsumer(ISchedulerService schedulerService, ILogger<ReservationCreatedConsumer> logger)
        {
            _schedulerService = schedulerService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IReservationCreated> context)
        {
            var msg = context.Message;

            _logger.LogInformation("Mensagem recebida: ReservationId={ReservationId}, Email={Email}",
                msg.ReservationId, msg.CustomerEmail);

            await _schedulerService.SchedulePaymentTimeoutAsync(msg.ReservationId, msg.CustomerName, msg.CustomerEmail);
        }
    }
}
