namespace SchedulerService.Domain.Entities
{
    public class ScheduledReservation
    {
        public Guid Id { get; set; }

        // Identificador da reserva original
        public Guid ReservationId { get; set; }

        // Dados do cliente
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        // Momento em que o agendamento foi criado
        public DateTime CreatedAt { get; set; }

        // Momento em que o timeout está programado para ocorrer
        public DateTime TimeoutScheduledFor { get; set; }

        // Valor simulado do pagamento (opcional)
        public decimal? Amount { get; set; }

        // Status do agendamento: Scheduled, Executed, Failed
        public string Status { get; set; } = "Scheduled";

        // Momento em que o evento foi realmente disparado (se aplicável)
        public DateTime? ExecutedAt { get; set; }
    }
}

