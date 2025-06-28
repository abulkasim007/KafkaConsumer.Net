namespace KafkaConsumer;

using Confluent.Kafka;
using System.Text.Json;

internal class KafkaMessageHandler(string topicName, ushort concurrency, IConsumer<Null, byte[]> consumer, DbContextProvider dbContextProvider)
   : ConcurrentKafkaMessageDispatcher<byte[]>(topicName, concurrency, consumer)
{
    public override async ValueTask HandleAsync(byte[] message)
    {
        DisburseCommand disburseCommand = JsonSerializer.Deserialize(message.AsSpan(), AppJsonSerializerContext.Default.DisburseCommand);

        LoanAggregateRoot newLoanAggregateRoot = new();

        newLoanAggregateRoot.CreateLoan(disburseCommand.UserContext);

        using MicrofinanceStateDbContext dbContext = dbContextProvider.Get();

        dbContext.LoanAggregateRoots.Add(newLoanAggregateRoot);

        await dbContext.SaveChangesAsync();
    }
}