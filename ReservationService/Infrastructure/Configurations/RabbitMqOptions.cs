namespace ReservationService.Infrastructure.Configurations
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = "RabbitMq"; // Chave da seção no appsettings

        public string Host { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
