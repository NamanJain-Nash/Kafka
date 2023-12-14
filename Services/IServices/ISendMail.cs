namespace Services.IServices
{
    public interface ISendMail
    {
        public Task<string> SendGraphAPI(List<string> toEmail, string subject, string body);
    }
}