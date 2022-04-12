using Microsoft.Extensions.Configuration;
using System;

namespace CityInfo.Infrastructure.Services
{
    public class CloudMailService : IMailService
    {
        public readonly string _mailTo = string.Empty;
        public readonly string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            _mailFrom = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"]; 
        }

        public void Send(string subject, string message)
        {
            //send mail - output to console window

            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
