using AppWeb.CyaConsultorias.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AppWeb.CyaConsultorias.Utilitaries
{
    public class EmailSender : IEmailSender
    {
        private SmtpClient Cliente { get; }
        private EmailSenderOptionsModel Options { get; }

        public EmailSender(IOptions<EmailSenderOptionsModel> options)
        {
            Options = options.Value;
            Cliente = new SmtpClient()
            {
                EnableSsl = Options.EnableSsl,
                UseDefaultCredentials = false,
                Host = Options.Host,
                Port = Options.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(Options.FromEmail, Options.Password),
            };
        }

        public Task SendEmailAsync(bool isCertification, string subject, string message)
        {
            var correo = new MailMessage
            {
                From = new MailAddress(Options.FromEmail)
            };
            if (isCertification)
            {
                correo.To.Add(Options.ToCertificationEmail);
            }
            else
            {
                correo.To.Add(Options.ToContactEmail);
            }
            correo.Subject = subject;
            correo.Body = message;

            return Cliente.SendMailAsync(correo);
        }
    }
}
