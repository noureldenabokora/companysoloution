using Demo.DAL.Models;
using Microsoft.Extensions.Options;
using routeone.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Demo.PL.Helpers
{
	public class SmsServices :ISmsServices
	{
		private readonly TwilioSettings _options;

		public SmsServices(IOptions<TwilioSettings> options)
		{
			_options = options.Value;
		}

		public MessageResource Send(SmsMessage sms)
		{
			TwilioClient.Init(_options.AccountSID, _options.AuthToken);
			var result = MessageResource.Create(
				body: sms.Body,
				from: new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
				to: sms.PhoneNumber
				);

			return result;
		}
	
	}
}
