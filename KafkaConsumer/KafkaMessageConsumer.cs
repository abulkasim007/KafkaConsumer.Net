namespace KafkaConsumer;

using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

internal class KafkaMessageConsumer(ProgramArguments programArguments, DbContextProvider dbContextProvider) : IHostedService
{
    private const int Concurrency = 50;
    private const string GroupId = "simple";
    private const string TopicName = "Microfinance.Commands.DisburseCommand";
    //private const string BootstrapServers = "10.42.53.125:19092,10.42.53.125:29092,10.42.53.125:39092";

    private readonly List<Task> kafkaListenerTasks = [];
    private readonly CancellationTokenSource cancellationTokenSource = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        StartConsumers(TopicName, dbContextProvider, cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            cancellationTokenSource.Cancel();

            await Task.WhenAll(kafkaListenerTasks);
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void StartConsumers(string topicName, DbContextProvider dbContextProvider, CancellationToken cancellationToken)
    {
        ConsumerConfig consumerConfig = new()
        {
            EnableAutoCommit = false,
            EnableAutoOffsetStore = false,
            GroupId = GroupId,
            AllowAutoCreateTopics = true,
            BootstrapServers = programArguments.KafkaServers, //BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Latest,
            PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky,
        };

        IConsumer<Null, byte[]> consumer = new ConsumerBuilder<Null, byte[]>(consumerConfig)
        .Build();

        consumer.Subscribe(topicName);

        KafkaMessageDispatcherBase<byte[]> kafkaMessageHandler = new KafkaMessageHandler(topicName, Concurrency, consumer, dbContextProvider);

        Task kafkaListenerTask = kafkaMessageHandler.StartAsync(cancellationToken);

        kafkaListenerTasks.Add(kafkaListenerTask);

        Console.WriteLine($"Started Kafka consumer for topic '{topicName}' with concurrency {Concurrency}.");
    }
}
