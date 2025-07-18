using System.Net.WebSockets;
using System.Collections.Concurrent;
using System.Text;

namespace Server.Services
{
    public class WebSocketManager : IWebSocketManager
    {
        private readonly ConcurrentBag<WebSocket> _sockets = new();

        public void AddSocket(WebSocket socket)
        {
            _sockets.Add(socket);
        }

        public async Task BroadcastMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);
            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
