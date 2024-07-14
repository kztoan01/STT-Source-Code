using Microsoft.AspNetCore.SignalR;

namespace controller.Hubs
{
    public class SignalRServer : Hub
    {
        public async Task JoinSpecificRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
            await Clients.Group(userConnection.Room).SendAsync("RecievedMessage", "admin", $"{userConnection.UserName} has joined");
        }
    }
}
