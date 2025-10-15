using MassTransit;
using ReservationService.Application.Services;

namespace ReservationService.Consumers
{
    public class ReservationCreatedConsumer : IConsumer<ReservationCreated>
    {
        /*
         Mostra que o serviço está apto a consumir eventos (mesmo que não seja necessário no ReservationService)
         apenas para teste. Mas em produção, esse consumer deveria estar no SchedulerService
         */
        public Task Consume(ConsumeContext<ReservationCreated> context)
        {
            var message = context.Message;
            Console.WriteLine($"Reserva recebida: {message.ReservationId} para {message.CustomerEmail}");
            return Task.CompletedTask;
        }
    }
}
