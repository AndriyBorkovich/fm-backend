using System.Net;
using FootballManager.Application.Contracts.Email;
using FootballManager.Application.Models.Email;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FootballManager.Infrastructure.EmailService;

public class EmailSender : IEmailSender
{
    public EmailSettings Settings { get; set; }
    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        Settings = emailSettings.Value;
    }

    public async Task<bool> SendEmail(EmailMessage email)
    {
        var client = new SendGridClient(Settings.ApiKey);
        var to = new EmailAddress(email.To);

        var from = new EmailAddress()
        {
            Email = Settings.FromAddress,
            Name = Settings.FromName
        };

        var message = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);

        var response = await client.SendEmailAsync(message);

        return response.IsSuccessStatusCode;
    }
}