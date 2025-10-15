namespace CustomerHistoryService.Domain
{
    public class PaymentRecord
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
