using ConsumerAPI.Model;
using ConsumerAPI.Model.Mail;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerAPI.Service.Interface
{
    public interface IGetQueueInfoService
    {
        Task SendErrorEmail([FromForm] MailRequest request);
        void GetConsumerInfo();
        Task CallAPIEndpoint(RequestParam model);


    }
}
