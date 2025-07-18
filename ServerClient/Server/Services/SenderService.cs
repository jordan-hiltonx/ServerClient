using System.Net.WebSockets;
using Server.Models;
using System.Text.Json;
using System.Text;
using Server.Interfaces;

namespace Server.Services;

public class SenderService : ISenderService
{
    private readonly ClientWebSocket _socket;
    
    public SenderService(ClientWebSocket socket)
    {
        _socket = socket ?? throw new ArgumentNullException(nameof(socket));
    }
    
    public async Task<Request> SendMessageAsync(string message)
    {
        var request = PrepareMessageAsync(message);
        // Simulate sending the message
        Console.WriteLine($"Sending message: {request} at {DateTime.Now}");
        
        // Create a new Request object with the message
        var sentMessage = new Request
        {
            Key = Guid.NewGuid(),
            Timestamp = DateTimeOffset.Now,
            Message = request,
            Status = "Sent"
        };
        
        // Serialize sentMessage to JSON for sending
        var sentMessageJson = JsonSerializer.Serialize(sentMessage);
        
        // Use the serialized JSON in _socket.SendAsync()
        var buffer = Encoding.UTF8.GetBytes(sentMessageJson);
        var segment = new ArraySegment<byte>(buffer);
        await _socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        
        // Set the status to "Sent"
        sentMessage.Status = "Sent";
        
        return sentMessage;
    }
    
    private string PrepareMessageAsync(string message)
    {
        var preparedMessage = new Request
        {
            Key = Guid.NewGuid(),
            Timestamp = DateTimeOffset.Now,
            Message = message,
            Status = "Prepared"
        };
        
        var preparedMessageJson = JsonSerializer.Serialize(preparedMessage);
        Console.WriteLine($"Prepared message: {preparedMessageJson}");
        return preparedMessageJson;
    }
}