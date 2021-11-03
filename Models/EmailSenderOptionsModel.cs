
namespace AppWeb.CyaConsultorias.Models
{
    public class EmailSenderOptionsModel
    {
        public int Port { get; set; }
        public string FromEmail { get; set; }
        public string ToContactEmail { get; set; }
        public string ToCertificationEmail { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
    }
}
