using System.Net;
using System.Net.Mail;

namespace courseappchallenge.Services;

public class EmailService : IEmailService
{
    public bool Send(string toName, string toEmail, string subject)
    {
        var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);
        smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;

        var mail = new MailMessage();

        mail.From = new MailAddress("cris_nor@hotmail.com", "crisnordev");
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.IsBodyHtml = true;

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }

}