namespace PhoneDirectory.API.Services
{
    public interface IMessagePublisher
    {
        void Publish<T>(T message);
    }
}
