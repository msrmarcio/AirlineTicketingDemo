namespace CustomerHistoryService.Domain
{
    public class CustomerHistory
    {
        public Guid Id { get; set; }
        public string CustomerEmail { get; set; }
        public List<ReservationRecord> Reservations { get; set; } = new();
        public List<PaymentRecord> Payments { get; set; } = new();
        public List<NotificationRecord> Notifications { get; set; } = new();
    }
}
