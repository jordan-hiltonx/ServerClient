using Microsoft.AspNetCore.Mvc;
using Server.Services;
using WebSocketManager = Server.Services.WebSocketManager;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/server")]
    public class SenderController : ControllerBase
    {
        private readonly IWebSocketManager _webSocketManager;
        
        public SenderController(IWebSocketManager webSocketManager)
        {
            _webSocketManager = webSocketManager ?? throw new ArgumentNullException(nameof(webSocketManager));
        }
        
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] string message)
        {
            try
            {
                await _webSocketManager.BroadcastMessageAsync(message);
                Console.WriteLine($"Message sent: {message} at {DateTime.Now}");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}