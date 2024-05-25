using FootballManager.Application.Models.Email;

namespace FootballManager.Application.Contracts.Email;

public interface IEmailSender
{
    Task<bool> SendEmail(EmailMessage email);
}