using Contracts;

namespace ReservationService.Application.Services
{

    public class ReservationCreated : IReservationCreated
    {
        public Guid ReservationId { get; init; }
        public string CustomerName { get; init; }
        public string CustomerEmail { get; init; }
    }

}
