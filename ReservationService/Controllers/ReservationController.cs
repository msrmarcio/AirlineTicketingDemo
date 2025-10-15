using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationService.Application.DTOs;
using ReservationService.Application.Interfaces;

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerName) ||
                string.IsNullOrWhiteSpace(request.CustomerEmail) ||
                request.Amount <= 0)
            {
                _logger.LogWarning("Dados inválidos recebidos na criação de reserva");
                return BadRequest("Nome, e-mail e valor devem ser preenchidos corretamente.");
            }

            try
            {
                var reservation = await _reservationService.CreateReservationAsync(
                    request.CustomerName,
                    request.CustomerEmail,
                    request.Amount);

                _logger.LogInformation("Reserva criada com sucesso: {ReservationId}", reservation.Id);

                return Ok(new
                {
                    reservation.Id,
                    reservation.CustomerName,
                    reservation.CustomerEmail,
                    reservation.Amount,
                    reservation.Status,
                    reservation.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao criar reserva");
                return StatusCode(500, "Erro interno ao processar a reserva.");
            }
        }
    }

}
