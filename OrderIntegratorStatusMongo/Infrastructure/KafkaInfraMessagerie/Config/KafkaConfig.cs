namespace Infrastructure.KafkaInfraMessagerie.Config
{
    public class KafkaConfig
    {
        public string? BootstrapServers { get; set; }
        public string? Topic { get; set; }
        public string? ConsumerTopic { get; set; }
        public string? PaymentTopic { get; set; }
        public string? IntegrationTopic { get; set; }
    }
}