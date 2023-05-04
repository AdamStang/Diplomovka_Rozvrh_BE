using backend.Models.RoomDomain;

namespace backend.Handlers.RoomDomain
{
    public interface IRoomCommandHandler
    {
        Task CreateRoom(Room room);
        Task DeleteRoom(string roomId);
    }
}