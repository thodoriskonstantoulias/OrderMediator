namespace OrderMediator.Services
{
    public interface IEmailService
    {
        public Task SendMailAsync(string body);
    }
}
