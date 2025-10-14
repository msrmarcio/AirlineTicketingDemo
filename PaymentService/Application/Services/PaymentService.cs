using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Messages;
using PaymentService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace PaymentService.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository paymentRepository, IPublishEndpoint publishEndpoint, ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<Payment> ProcessPaymentAsync(Guid reservationId, string customerEmail, decimal amount)
        {
            /* Simula processamento de pagamento com 80% de chance de sucesso | Probabilidade: 8/10 = 80%*/

            var success = new Random().Next(0, 10) <= 7;

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                ReservationId = reservationId,
                Amount = amount,
                Status = success ? "Approved" : "Rejected",
                ProcessedAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            if (success)
            {
                await _publishEndpoint.Publish(new PaymentApproved(
                    reservationId,
                    customerEmail,
                    amount,
                    payment.ProcessedAt));
            }
            else
            {
                await _publishEndpoint.Publish(new PaymentRejected(
                    reservationId,
                    customerEmail,
                    amount,
                    payment.ProcessedAt));
            }

            return payment;
        }

        public async Task<Payment?> GetPaymentByReservationIdAsync(Guid reservationId)
        {
            return await _paymentRepository.GetByReservationIdAsync(reservationId);
        }
         
    }
}
