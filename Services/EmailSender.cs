using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace courseappchallenge.Services;

public class EmailSender : IEmailSender
{
    private readonly AuthMessageSenderOptions _options;
    
    public EmailSender(AuthMessageSenderOptions options)
    {
        _options = options;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_options.SendGridKey))
            throw new Exception("Null SendGridKey");

        var emailClient = new SendGridClient(_options.SendGridKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress("cris_nor@hotmail.com", "Password Recovery"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        await emailClient.SendEmailAsync(msg);
    }
}