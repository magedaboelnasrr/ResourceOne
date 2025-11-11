namespace ResourceOne.Services
{
    public interface IEmailServiceee
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
