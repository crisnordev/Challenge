using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CourseAppChallenge.Services;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Configuration.SendGridKey.SendGridApiKey)) throw new Exception("Null SendGridKey");

        await Execute(Configuration.SendGridKey.SendGridApiKey, subject, message, toEmail);
    }

    public static async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress("cris_nor@hotmail.com", "Cristiano Noronha"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        msg.SetClickTracking(false, false);
        await client.SendEmailAsync(msg);
    }
}