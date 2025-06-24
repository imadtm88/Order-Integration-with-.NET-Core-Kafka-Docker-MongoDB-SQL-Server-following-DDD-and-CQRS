namespace Infrastructure.KafkaInfraMessagerie.Config
{
    public class KafkaConfig
    {
        public string? BootstrapServers { get; set; }
        public string? Topic { get; set; }
        public string? ProducerTopic { get; set; }
    }
}