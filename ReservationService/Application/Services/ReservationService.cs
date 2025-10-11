
using MassTransit;
using ReservationService.Application.Interfaces;
using ReservationService.Domain.Entities;
using ReservationService.Messages;

namespace ReservationService.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IReservationRepository _repository;

        public ReservationService(IPublishEndpoint publishEndpoint, IReservationRepository repository)
        {
            _publishEndpoint = publishEndpoint;
            _repository = repository;
        }

        public async Task<Guid> CreateReservationAsync(string customerName, string customerEmail)
        {
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                CustomerName = customerName,
                CustomerEmail = customerEmail
            };

            await _repository.AddAsync(reservation);

            await _publishEndpoint.Publish(new ReservationCreated(reservation.Id, customerName, customerEmail));

            return reservation.Id;
        }
    }
}
