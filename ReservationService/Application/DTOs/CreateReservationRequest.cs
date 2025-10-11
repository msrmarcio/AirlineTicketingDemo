namespace ReservationService.Application.DTOs
{
    public class CreateReservationRequestDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }
}
