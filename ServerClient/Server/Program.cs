using System.Net.WebSockets;
using Server.Services;
using WebSocketManager = Server.Services.WebSocketManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddScoped<ClientWebSocket>();
builder.Services.AddSingleton<IWebSocketManager, WebSocketManager>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var url = "http://localhost:5000";
    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var wsManager = context.RequestServices.GetRequiredService<WebSocketManager>();
        wsManager.AddSocket(webSocket);
        // Keep the connection open
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
            }
        }
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();
