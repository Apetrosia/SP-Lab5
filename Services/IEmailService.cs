namespace GreenswampRazorPages.Services;

public interface IEmailService
{
    Task SendSubscriptionConfirmationAsync(string toEmail);
}