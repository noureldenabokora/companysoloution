using Demo.DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace Demo.PL.Helpers
{
	public interface ISmsServices
	{
		//method return message resource
		MessageResource Send(SmsMessage sms);
	}
}
