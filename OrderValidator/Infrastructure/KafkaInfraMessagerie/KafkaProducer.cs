using Confluent.Kafka;
using Infrastructure.KafkaInfraMessagerie.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaProducer
    {
        private readonly ProducerConfig _config;
        private readonly string? _topic;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaProducer> logger)
        {
            _config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers
            };
            _topic = "vpi-topic";
            _logger = logger;
        }

        public async Task ProduceAsync(string message, string id)
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            try
            {
                var fullMessage = JsonConvert.SerializeObject(new { Id = id, Message = message });
                var result = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = fullMessage });
                _logger.LogInformation($"Message sent to topic {_topic} at offset {result.Offset}: {fullMessage}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Error producing message: {e.Error.Reason}");
            }
        }
    }
}