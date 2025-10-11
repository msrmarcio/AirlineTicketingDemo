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


        // POST: ReservationController/Create
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerName))
                return BadRequest(new { message = "CustomerName is required" });

            try
            {
                var reservationId = await _reservationService.CreateReservationAsync(
                    request.CustomerName, request.CustomerEmail);

                _logger.LogInformation(
                    "Reservation created: {ReservationId} for {CustomerName}", reservationId, request.CustomerName);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    reservationId,
                    message = "Reservation created successfully (event published)"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reservation");

                return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new { message = "Internal error in process reservation" });
            } 
        }
    }

}
