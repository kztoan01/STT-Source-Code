namespace service.Hub;

public interface IRoomHub
{
    Task AlertToRoom(string username);
    
}