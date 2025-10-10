
using ReservationService.Application.Interfaces;
using MassTransit;
using ReservationService.Messages;

namespace ReservationService.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ReservationService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> CreateReservationAsync(string customerName)
        {
            var reservationId = Guid.NewGuid();

            await _publishEndpoint.Publish(new ReservationCreated(reservationId, customerName));

            return reservationId;
        }
    }
}
