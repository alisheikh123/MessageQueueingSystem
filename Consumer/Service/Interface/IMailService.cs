using ConsumerAPI.Model.Mail;

namespace ConsumerAPI.Service.Interface
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
