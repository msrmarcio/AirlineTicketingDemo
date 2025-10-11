using ReservationService.Domain.Entities;

namespace ReservationService.Application.Interfaces
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
    }

}
