namespace ShopWeb.Models
{
    public interface IEmailSendercs
    {
        Task SendMailAsync(string name, string email, string subject, string message);
    }
}
