using Confluent.Kafka;
using Infrastructure.KafkaInfraMessagerie.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.KafkaInfraMessagerie
{
    public class KafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IOptions<KafkaConfig> kafkaConfigOptions, ILogger<KafkaProducer> logger)
        {
            var config = kafkaConfigOptions.Value;
            _producer = new ProducerBuilder<Null, string>(new ProducerConfig
            {
                BootstrapServers = config.BootstrapServers
            }).Build();
            _topic = config.ProducerTopic;
            _logger = logger;
        }

        public async Task ProduceMessageAsync(string id, string message)
        {
            var value = JsonConvert.SerializeObject(new { Id = id, Message = message });

            try
            {
                var dr = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = value });
                _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
