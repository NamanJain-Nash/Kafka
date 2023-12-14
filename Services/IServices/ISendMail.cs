namespace Services.IServices
{
    public interface ISendMail
    {
        public Task<string> SendGraphAPI(List<string> toEmail, string subject, string body);
        public Task<string> SendEmailSMTP(List<string> toEmail, string subject, string body);
    }
}