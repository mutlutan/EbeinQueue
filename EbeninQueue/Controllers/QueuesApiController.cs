using Microsoft.AspNetCore.Mvc;

namespace EbeninQueue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueuesApiController : ControllerBase
    {
        public QueuesApiController()
        {
        }

        #region Memory Info/Optimize
        public decimal GetTotalMemory()
        {
            return Convert.ToInt32(GC.GetTotalMemory(false) / (1024 * 1024));
        }

        [HttpGet("GetMemory")]
        public IActionResult GetMemory()
        {
            return Ok(this.GetTotalMemory().ToString());
        }

        public IActionResult Collect()
        {
            string beforeMemory = this.GetTotalMemory() + " MB";
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            string nowMemory = this.GetTotalMemory() + " MB";

            var obj = new
            {
                BeforeMemory = beforeMemory,
                NowMemory = nowMemory,
            };

            return Ok(obj);
        }
        #endregion

        #region Queue

        [HttpGet("NewChannel/{channel}")]
        public IActionResult NewChannel(string channel)
        {
            return Ok(QueuesHelper.NewChannel(channel));
        }

        [HttpGet("ChannelInfo")]
        public IActionResult ChannelInfo()
        {
            return new ContentResult()
            {
                Content = QueuesHelper.ChannelInfoHtml().Data?.ToString(),
                ContentType = "text/html",
            };
        }

        [HttpGet("QueueInfo/{channel}")]
        public IActionResult QueueInfo(string channel)
        {
            return new ContentResult()
            {
                Content = QueuesHelper.QueueInfoHtml(channel).Data?.ToString(),
                ContentType = "text/html",
            };
        }

        [HttpPost("Enqueue")]
        [AuthenticateRequired]
        public IActionResult Enqueue([FromBody] MoEnqueue moEnqueue)
        {
            MoResponse<object> response = new();

            if (!string.IsNullOrEmpty(moEnqueue.Channel) && !string.IsNullOrEmpty(moEnqueue.Element))
            {
                response = QueuesHelper.Enqueue(moEnqueue.Channel, moEnqueue.Element, moEnqueue.Periority);

                if (!response.IsSuccess)
                {
                    Thread.Sleep(500);
                    var response2 = QueuesHelper.Enqueue(moEnqueue.Channel, moEnqueue.Element, moEnqueue.Periority);
                    List<string> msg = response2.Messages;
                    response = response2;
                    response.Messages.AddRange(msg);
                }

                if (!response.IsSuccess)
                {
                    Thread.Sleep(1000);
                    var response3 = QueuesHelper.Enqueue(moEnqueue.Channel, moEnqueue.Element, moEnqueue.Periority);
                    List<string> msg = response3.Messages;
                    response = response3;
                    response.Messages.AddRange(msg);
                }

                if (!response.IsSuccess)
                {
                    Thread.Sleep(1500);
                    var response4 = QueuesHelper.Enqueue(moEnqueue.Channel, moEnqueue.Element, moEnqueue.Periority);
                    List<string> msg = response4.Messages;
                    response = response4;
                    response.Messages.AddRange(msg);
                }
            }

            return Ok(response);
        }

        [HttpPost("Dequeue")]
        [AuthenticateRequired]
        public IActionResult Dequeue([FromBody] MoInput moInput)
        {
            return Ok(QueuesHelper.Dequeue(moInput.Channel));
        }

        [HttpPost("Peek")]
        [AuthenticateRequired]
        public IActionResult Peek([FromBody] MoInput moInput)
        {
            return Ok(QueuesHelper.Peek(moInput.Channel));
        }

        #endregion
    }
}
