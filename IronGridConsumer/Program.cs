using Confluent.Kafka;
using IronGridConsumer.IronGridServices;
using IronGridConsumer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string connectionString = configuration["ConnectionStrings:testDb"]!;

var services = new ServiceCollection();

services.AddDbContext<IronGridDbContext>(opt => opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
services.AddScoped<ConsumerServices>();
var serviceProvider = services.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<IronGridDbContext>().Database.EnsureCreated();
}
var consumerConfig = new ConsumerConfig
{
    BootstrapServers = configuration["Kafka:BootstrapServices"],
    GroupId = configuration["Kafka:GroupId"],
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = false,
    AllowAutoCreateTopics = true
};

using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
try
{
    await ConsumeTopicAsync(configuration["Kafka:Topics:UAV"]!);
    await ConsumeTopicAsync(configuration["Kafka:Topics:PerimeterSensor"]!);
}
catch (OperationCanceledException)
{
    Console.WriteLine("\nshutting down");
}
finally
{
    consumer.Close();
    Console.WriteLine("consumer closed");
}

async Task ConsumeTopicAsync(string topic)
{
    Console.WriteLine($"\n subscribed to: {topic}");
    consumer.Subscribe(topic);

    int emptyReads = 0;
    while (emptyReads < 5)
    {
        try
        {
            var result = consumer.Consume(TimeSpan.FromSeconds(1));
            if (result is null || result.Message?.Value == null)
            {
                emptyReads++;
                continue;
            }

            emptyReads = 0;
            Console.WriteLine($"{DateTime.Now} received from topic: {result.Topic}");

            using var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ConsumerServices>();

            bool isSuccess = false;
            if (topic == configuration["Kafka:Topics:PerimeterSensor"])
            {
                isSuccess = await service.ProcessPerimeterSensorModelAsync(result.Message.Value);
            }
            else if (topic == configuration["Kafka:Topics:UAV"])
            {
                isSuccess = await service.ProcessUAVModelAsync(result.Message.Value);
            }
            if (isSuccess)
            {
                consumer.Commit(result);
            }
        }

        catch (ConsumeException ex)
        {
            Console.WriteLine($"waiting: {ex.Error.Reason}");
            await Task.Delay(1000);
        }
    }

    consumer.Unsubscribe();
    Console.WriteLine($"finished topic: {topic}");
}
