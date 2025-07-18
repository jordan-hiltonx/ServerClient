namespace Server.Models;

public class Request
{
    public Guid Key { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
}