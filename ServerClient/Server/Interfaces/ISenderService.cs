using Server.Models;

namespace Server.Interfaces;

public interface ISenderService
{
    Task<Request> SendMessageAsync(string message);
}