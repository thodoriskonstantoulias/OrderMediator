namespace OrderMediator.Services
{
    public class EmailService : IEmailService
    {
        // Mock here
        // Steps - We decide on an email provider, get the appropriate api keys - add them to configuration
        // and send the mail here to the account manager
        public Task SendMailAsync(string body)
        {
            return Task.CompletedTask;
           // throw new NotImplementedException();
        }
    }
}
