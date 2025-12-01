namespace TTSS.Game.Analysis.Api.Services;
public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
}
