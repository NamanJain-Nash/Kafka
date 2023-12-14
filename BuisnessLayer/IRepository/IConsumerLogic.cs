namespace BuisnessLayer.IRepository
{
    public interface IConsumerLogic
    {
        public Task<string> SendMail(string message);
    }
}