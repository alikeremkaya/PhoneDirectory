namespace Report.Application.Interfaces;

public interface IMessageBus : IDisposable
{
    void PublishMessage<T>(T message, string queueName);
    void Subscribe<T>(string queueName, Action<T> onMessage);

}
