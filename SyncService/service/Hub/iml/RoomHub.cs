using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace service.Hub.iml;

public class RoomHub : Hub<IRoomHub>
{
    private static readonly ConcurrentDictionary<string, ConcurrentBag<string>> GroupConnections = new ConcurrentDictionary<string, ConcurrentBag<string>>();
    public async Task JoinRoom(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
        var connections = GroupConnections.GetOrAdd(groupName, _ => new ConcurrentBag<string>());
        connections.Add(Context.ConnectionId);
    }

    public async Task AlertToRoom(string groupName, string username)
    {
        await JoinRoom(groupName);
        await Clients.Group(groupName).AlertToRoom(username);
    }

    public async Task LeaveRoom(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        if (GroupConnections.TryGetValue(groupName, out var connections))
        {
            connections = new ConcurrentBag<string>(connections.Except(new[] { Context.ConnectionId }));
            if (connections.IsEmpty)
            {
                GroupConnections.TryRemove(groupName, out _);
            }
            else
            {
                GroupConnections[groupName] = connections;
            }
        }
    }

    public async Task DeleteRoom(string groupName)
    {
        if (GroupConnections.TryRemove(groupName, out var connections))
        {
            foreach (var connectionId in connections)
            {
                await Groups.RemoveFromGroupAsync(connectionId, groupName);
            }
        }
    }
}