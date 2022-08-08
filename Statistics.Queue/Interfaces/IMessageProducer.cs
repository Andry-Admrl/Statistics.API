namespace Statistics.Queue.Interfaces
{
    public interface IMessageProducer
    {
        public void SendProductMessage<T>(T message);
    }
}
