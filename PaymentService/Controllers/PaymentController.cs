using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Interfaces;
using PaymentService.Application.DTOs;
using MassTransit.Testing.Implementations;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
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
            if (request.ReservationId == Guid.Empty || request.Amount <= 0)
                return BadRequest(new { message = "Invalid data" });

            try
            {
                var success = await _paymentService.ProcessPaymentAsync(request.ReservationId, request.Amount);

                if (success)
                {
                    return Ok(new { message = "Payment processed successfully (event published)" });
                }
                else
                {
                    /* Simulação: mesmo que o pagamento falhe, retornamos 402 (Payment Required)
                     * para indicar que o pagamento não foi concluído.
                     * Em um cenário real, você poderia retornar diferentes códigos de status
                     * dependendo do motivo da falha (ex: 402, 403, 500, etc).
                     * Obs.: O código de status HTTP 402 Payment Required é um termo despadronizado para respostas de Status, 
                     * podendo-se ter usos futuros. 
                     * RFC 7231, sessão 6.5.2: 402 Payment Required
                     */
                    return StatusCode(StatusCodes.Status402PaymentRequired, new { message = "Payment failed (event published)" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for ReservationId: {ReservationId}", request.ReservationId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = "Internal error in process payment" });
            }
        }
    }
}
