using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace CityInfo.Infrastructure.Services
{
    public class LocalMailService : IMailService
    {
        public readonly string _mailTo = string.Empty;
        public readonly string _mailFrom = string.Empty;

        public LocalMailService()
        {

        }
        public LocalMailService(IConfiguration configuration)
        {
            _mailFrom = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            //send mail - output to console window

            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
