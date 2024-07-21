namespace service.Hub;

public interface IRoomHub
{
     Task AlertToRoom(string groupName, string username);
    Task Chat(string groupName, string username, string content);
    Task MusicStatus(string status, string musicName, string musicUrl);

    Task MusicPlaytime(double playtime);

    Task OnDeleteRoom(string groupName, string username);
    Task OnLeaveRoom(string groupName, string username);

    Task OnAddRoomMusic(string groupName, string musicId);

    Task OnRemoveRoomMusic(string groupName, string musicName);
    Task UpdateParticipantsList();
    Task UpdateMusicsList();
}