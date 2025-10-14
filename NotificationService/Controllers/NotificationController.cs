using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Interfaces;
using NotificationService.Application.DTOs;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet("{reservationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotifications(Guid reservationId)
        {
            if (reservationId == Guid.Empty)
                return BadRequest(new { message = "Invalid reservation ID" });

            try
            {
                var notifications = await _notificationService.GetNotificationsByReservationIdAsync(reservationId);

                if (notifications == null || notifications.Count == 0)
                    return NotFound(new { message = "No notifications found for this reservation" });

                var response = notifications.Select(n => new NotificationResponseDto
                {
                    Id = n.Id,
                    ReservationId = n.ReservationId,
                    CustomerEmail = n.CustomerEmail,
                    Type = n.Type,
                    Status = n.Status,
                    Message = n.Message,
                    SentAt = n.SentAt
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notifications for ReservationId: {ReservationId}", reservationId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal error" });
            }
        }
    }
}
