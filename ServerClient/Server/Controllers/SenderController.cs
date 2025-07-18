using Microsoft.AspNetCore.Mvc;
using Server.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/server")]
    public class SenderController : ControllerBase
    {
        private readonly ISenderService _senderService;
        
        public SenderController(ISenderService senderService)
        {
            _senderService = senderService ?? throw new ArgumentNullException(nameof(senderService));
        }
        
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            await _senderService.SendMessageAsync(message);
            Console.WriteLine($"Message sent: {message} at {DateTime.Now}");
            
            return Ok();
        }
    }
}