using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace service.Hub.iml;

public class RoomHub : Hub<IRoomHub>
{
    private static readonly ConcurrentDictionary<string, ConcurrentBag<string>> GroupConnections = new();

    public async Task JoinRoom(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var connections = GroupConnections.GetOrAdd(groupName, _ => new ConcurrentBag<string>());
        connections.Add(Context.ConnectionId);
    }

    public async Task AlertToRoom(string groupName, string username)
    {
        await JoinRoom(groupName);
        await Clients.Group(groupName).AlertToRoom(groupName, username);
    }
    public async Task Chat(string groupName, string username, string content)
    {
        //await JoinRoom(groupName);
        await Clients.Group(groupName).Chat(groupName, username, content);
    }
    public async Task UpdateParticipantsList(string groupName, string username)
    {
        await Clients.Group(groupName).UpdateParticipantsList();
    }


    public async Task UpdateMusicsList(string groupName)
    {
        await Clients.Group(groupName).UpdateMusicsList();
    }

    public async Task OnLeaveRoom(string groupName, string username)
    {
        await Clients.Group(groupName).OnLeaveRoom(groupName, username);
    }

    public async Task OnAddRoomMusic(string groupName, string musicName)
    {
        await Clients.Group(groupName).OnAddRoomMusic(groupName, musicName);
    }

    public async Task OnRemoveRoomMusic(string groupName, string musicName)
    {
        await Clients.Group(groupName).OnRemoveRoomMusic(groupName, musicName);
    }

    public async Task MusicPlaytime(string groupName, double playtime)
    {
        await Clients.Group(groupName).MusicPlaytime(playtime);
    }

        public async Task MusicStatus(string groupName, string status, string musicName, string musicUrl)
        {
            await Clients.Group(groupName).MusicStatus(status, musicName, musicUrl);
        }

    public async Task LeaveRoom(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        if (GroupConnections.TryGetValue(groupName, out var connections))
        {
            connections = new ConcurrentBag<string>(connections.Except(new[] { Context.ConnectionId }));
            if (connections.IsEmpty)
                GroupConnections.TryRemove(groupName, out _);
            else
                GroupConnections[groupName] = connections;
        }
    }

    public async Task DeleteRoom(string groupName)
    {
        if (GroupConnections.TryRemove(groupName, out var connections))
            foreach (var connectionId in connections)
                await Groups.RemoveFromGroupAsync(connectionId, groupName);
    }
}