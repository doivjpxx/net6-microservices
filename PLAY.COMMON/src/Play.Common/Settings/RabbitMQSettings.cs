namespace Play.Catalog.Service.Settings;

public class RabbitMQSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    
    public string ConnectionString => $"amqp://{Host}:{Port}";
}