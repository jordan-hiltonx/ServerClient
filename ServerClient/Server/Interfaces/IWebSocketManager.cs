using System.Net.WebSockets;

namespace Server.Services;

public interface IWebSocketManager
{
    void AddSocket(WebSocket socket);
    Task BroadcastMessageAsync(string message);
}