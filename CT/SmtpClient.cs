using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class SmtpClient
{
    private readonly System.Net.Mail.SmtpClient _smtpClient;

    public SmtpClient(string host, int port, string username, string password)
    {
        _smtpClient = new System.Net.Mail.SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };
    }

    public async Task SendMailAsync(string from, string to, string subject, string body)
    {
        using (MailMessage mailMessage = new MailMessage(from, to))
        {
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
