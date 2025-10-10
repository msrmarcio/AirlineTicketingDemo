namespace ReservationService.Application.Interfaces
{
    public interface IReservationService
    {
        Task<Guid> CreateReservationAsync(string customerName);
    }
}
