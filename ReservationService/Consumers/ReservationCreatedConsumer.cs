using MassTransit;
using ReservationService.Messages;

namespace ReservationService.Consumers
{
    public class ReservationCreatedConsumer : IConsumer<ReservationCreated>
    {
        public Task Consume(ConsumeContext<ReservationCreated> context)
        {
            var message = context.Message;
            Console.WriteLine($"Reserva recebida: {message.ReservationId} para {message.CustomerEmail}");
            return Task.CompletedTask;
        }
    }
}
