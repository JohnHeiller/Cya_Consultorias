using System.Threading.Tasks;

namespace AppWeb.CyaConsultorias.Utilitaries
{
    public interface IEmailSender
    {
        Task SendEmailAsync(bool isCertification, string subject, string message);
    }
}
