namespace Producor_Web_API.Interface
{
    public interface IRabitMQProducer
    {
        public void SendProductMessage<T>(T message);
    }
}
