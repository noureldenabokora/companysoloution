using Demo.DAL.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using routeone.Settings;
using System.Net;
using System.Net.Mail;

namespace routeone.Helpers
{
	public  class EmailSettings : IEmailSettings
	{
        private readonly MailSettings options;

        public EmailSettings(IOptions<MailSettings> options)
        {
            this.options = options.Value;
        }
        public  void SendEmail(Email email)
		{
			var mail = new MimeMessage
			{
				Sender = MailboxAddress.Parse(options.Email),
				Subject = email.Subject,
			};

			mail.To.Add(MailboxAddress.Parse(email.To));
			var builder =  new BodyBuilder();
			builder.HtmlBody = email.Body;
			mail.Body = builder.ToMessageBody();
			mail.From.Add(new  MailboxAddress(options.DiasplayName,options.Email));
			using var smtp = new SmtpClient();
		/*	smtp.Connect(options.Host, options.Port,SecureSocketOptions.StartTls);
			smtp.Authentication(options.Email,options.Password);
			smtp.Send(mail);
			smtp.Disconnect(true);
*/






			///old way of  code to send mails 
			
		/*	var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("nourabokora@gmail.com", "lmixdqdauhulvaqf");
			client.Send("nourabokora@gmial.com", email.To, email.Subject, email.Body);*/
		}
	}
}
