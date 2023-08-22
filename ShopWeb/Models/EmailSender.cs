using System.Net;
using System.Net.Mail;

namespace ShopWeb.Models
{
    public class EmailSender : IEmailSendercs
    {
        public Task SendMailAsync(string name, string email, string subject, string message)
        {
            var mail = "thanhlangtuyen@gmail.com";
            var pw = "chich1412";
            message = "Hello " + name + ".\nThank you for your submission!\nYour message is: " + message;

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };
            return client.SendMailAsync(
                new MailMessage(from: mail,
                                to: email,
                                subject,
                                message
                ));

        }
    }
}
