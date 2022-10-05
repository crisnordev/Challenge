using System.Net;
using System.Net.Mail;
using System.Net.Http;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace courseappchallenge.Services;

public class EmailService : IEmailService
{
    public async Task SendAsync(string toName, string toEmail, string subject, string body)
    {
        var apiKey = Configuration.SendGridKey.Token;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("cris_nor@hotmail.com", "crisnordev");
        var to = new EmailAddress(toEmail, toName);
        var htmlContent = @$"<strong> {body} </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, htmlContent);
        await client.SendEmailAsync(msg).ConfigureAwait(false);
    }
}