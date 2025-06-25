namespace KafkaConsumer;

public abstract class KafkaMessageDispatcherBase<T>
{
    public abstract ValueTask HandleAsync(T message);

    public abstract Task StartAsync(CancellationToken cancellationToken);
}