using Confluent.Kafka;
namespace IronGridProducer.IronGridServices;

public class KafkaProducerServices
{
    private readonly string _bootstrapServices;
    private readonly IProducer<Null, string> _producer;
    public KafkaProducerServices(string bootstrapServices)
    {
        _bootstrapServices = bootstrapServices;
        var config = new ProducerConfig { BootstrapServers = bootstrapServices };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task SendAsync(string topicName, string content)
    {

        var message = new Message<Null, string> { Value = content };
        var result = await _producer.ProduceAsync(topicName, message);
    }
}   