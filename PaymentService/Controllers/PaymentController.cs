using MassTransit.Testing.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequestDto request)
        {
            if (request.ReservationId == Guid.Empty || request.Amount <= 0 || string.IsNullOrWhiteSpace(request.CustomerEmail))
            {
                _logger.LogWarning("Invalid payment request: {@Request}", request);
                return BadRequest(new { message = "Invalid data" });
            }

            try
            {
                Payment payment = await _paymentService.ProcessPaymentAsync(
                    request.ReservationId,
                    request.CustomerEmail,
                    request.Amount);

                if (payment.Status == "Approved")
                {
                    return Ok(new
                    {
                        message = "Payment processed successfully",
                        paymentId = payment.Id,
                        status = payment.Status,
                        processedAt = payment.ProcessedAt
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status402PaymentRequired, new
                    {
                        message = "Payment failed",
                        paymentId = payment.Id,
                        status = payment.Status,
                        processedAt = payment.ProcessedAt
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for ReservationId: {ReservationId}", request.ReservationId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Internal error while processing payment"
                });
            }
        }

        [HttpGet("{reservationId}")]
        public async Task<IActionResult> GetPaymentByReservationId(Guid reservationId)
        {
            if (reservationId == Guid.Empty)
                return BadRequest(new { message = "Invalid reservation ID" });

            var payment = await _paymentService.GetPaymentByReservationIdAsync(reservationId);

            if (payment == null)
                return NotFound(new { message = "Payment not found for this reservation" });

            return Ok(new
            {
                paymentId = payment.Id,
                reservationId = payment.ReservationId,
                amount = payment.Amount,
                status = payment.Status,
                processedAt = payment.ProcessedAt
            });
        }

    }
}
