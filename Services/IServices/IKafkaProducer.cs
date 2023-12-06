namespace Services.IServices;

public interface IKafkaProducer
{
    public void Dispose();
    public Task<string> ProduceAsync(string message, string topicName);

}