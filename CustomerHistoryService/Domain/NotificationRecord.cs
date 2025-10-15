namespace CustomerHistoryService.Domain
{
    public class NotificationRecord
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime SentAt { get; set; }
    }
}
