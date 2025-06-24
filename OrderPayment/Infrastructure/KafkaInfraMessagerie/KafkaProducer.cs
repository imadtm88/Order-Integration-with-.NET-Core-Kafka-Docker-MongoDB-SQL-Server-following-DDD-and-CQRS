using Confluent.Kafka;
using Infrastructure.KafkaInfraMessagerie.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly IProducer<Null, string> _producer;
        private readonly string? _producerTopic;

        public KafkaProducer(ILogger<KafkaProducer> logger, IOptions<KafkaConfig> kafkaConfigOptions)
        {
            _logger = logger;
            var config = kafkaConfigOptions.Value;
            _producer = new ProducerBuilder<Null, string>(new ProducerConfig
            {
                BootstrapServers = config.BootstrapServers
            }).Build();
            _producerTopic = config.ProducerTopic;
        }

        public async Task SendMessageAsync(object message)
        {
            var messageValue = JsonConvert.SerializeObject(message);
            try
            {
                var dr = await _producer.ProduceAsync(_producerTopic, new Message<Null, string> { Value = messageValue });
                _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
