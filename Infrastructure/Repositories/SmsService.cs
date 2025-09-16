using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Repositories
{
    public class TwilioSettings
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }

        public string FromPhoneNumber { get; set; }
    }
    public class SmsService : ISmsService
    {
        private readonly TwilioSettings settings;
        public SmsService(IOptions<TwilioSettings> options)
        {
            settings = options.Value;
            TwilioClient.Init(settings.AccountSid, settings.AuthToken);
        }
        public async Task SendSmsAsync(string to,string message)
        {
            await MessageResource.CreateAsync(
                to: new PhoneNumber(to),
                from: new PhoneNumber(settings.FromPhoneNumber),
                body: message
                );
        }

    }
}
