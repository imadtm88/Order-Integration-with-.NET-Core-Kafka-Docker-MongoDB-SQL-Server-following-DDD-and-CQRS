using Confluent.Kafka;
using Infrastructure.KafkaInfraMessagerie.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaProducer : IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly KafkaConfig _config;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IOptions<KafkaConfig> config, ILogger<KafkaProducer> logger)
        {
            _config = config.Value;
            _logger = logger;
            var producerConfig = new ProducerConfig { BootstrapServers = _config.BootstrapServers };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task ProduceAsync<T>(T messageObject)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };
            var jsonMessage = JsonSerializer.Serialize(messageObject, jsonOptions);

            try
            {
                var result = await _producer.ProduceAsync(_config.Topic, new Message<Null, string> { Value = jsonMessage });
                _logger.LogInformation($"Message sent to {result.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError($"Delivery failed: {ex.Error.Reason}");
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}
