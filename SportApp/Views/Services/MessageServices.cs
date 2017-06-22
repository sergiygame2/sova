using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace SportApp.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Sergiy", "sergiy.surename@gmail.com"));
            msg.To.Add(new MailboxAddress("Dear User", email));
            msg.Subject = "How you doin? Confirm email";

            var builder = new BodyBuilder {HtmlBody = message};
            msg.Body = builder.ToMessageBody();

            await Task.FromResult(0);
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("sergiy.surename@gmail.com", "1997sergiy");
                
                client.Send(msg);
                client.Disconnect(true);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
