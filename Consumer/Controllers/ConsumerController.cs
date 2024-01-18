using ConsumerAPI.Model.TimeFeature;
using ConsumerAPI.Service;
using ConsumerAPI.Service.Interface;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

namespace ConsumerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private readonly IGetQueueInfoService _getQueueInfoService;
        public ConsumerController(IGetQueueInfoService getQueueInfoService)
        {
            _getQueueInfoService = getQueueInfoService;
           
           
        }

        [HttpGet]
        public async Task<ActionResult> GetQueueInfo()
        {
            var timerManager = new TimerManager(() => _getQueueInfoService.GetConsumerInfo());
            
            return Ok();
        }
      
    }
}
