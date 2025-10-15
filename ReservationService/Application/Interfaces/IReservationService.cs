using ReservationService.Domain.Entities;

namespace ReservationService.Application.Interfaces
{
    public interface IReservationService
    {
        Task<Reservation> CreateReservationAsync(string customerName, string customerEmail, decimal amount);
    }
}
