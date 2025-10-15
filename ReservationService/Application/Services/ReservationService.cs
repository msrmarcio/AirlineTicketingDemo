
using Contracts;
using MassTransit;
using ReservationService.Application.Interfaces;
using ReservationService.Domain.Entities;

namespace ReservationService.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IReservationRepository _repository;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(IPublishEndpoint publishEndpoint, IReservationRepository repository, ILogger<ReservationService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _repository = repository;
            _logger = logger;
        }

        public async Task<Reservation> CreateReservationAsync(string customerName, string customerEmail, decimal amount)
        {
            _logger.LogInformation("Iniciando criação de reserva para {CustomerName} ({CustomerEmail}) - Valor: {Amount}",
                customerName, customerEmail, amount);

            try
            {
                var reservation = new Reservation
                {
                    Id = Guid.NewGuid(),
                    CustomerName = customerName,
                    CustomerEmail = customerEmail,
                    Amount = amount,
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.AddAsync(reservation);

                _logger.LogInformation("Reserva criada com sucesso: ReservationId={ReservationId}", reservation.Id);

                await _publishEndpoint.Publish<IReservationCreated>(new ReservationCreated
                {
                    ReservationId = reservation.Id,
                    CustomerName = reservation.CustomerName,
                    CustomerEmail = reservation.CustomerEmail
                });

                _logger.LogInformation("Evento ReservationCreated publicado: {ReservationId} para {CustomerEmail}",
                    reservation.Id, reservation.CustomerEmail);

                return reservation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar reserva para {CustomerEmail}", customerEmail);
                throw; // ou retornar erro controlado, dependendo do seu padrão
            }
        }
    }
}
