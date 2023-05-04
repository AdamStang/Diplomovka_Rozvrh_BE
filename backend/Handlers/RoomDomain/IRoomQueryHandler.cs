using backend.Models.RoomDomain;

namespace backend.Handlers.RoomDomain
{
    public interface IRoomQueryHandler
    {
        Task<List<Room>> GetAllRooms();
        Task<long> GetRoomsCount();
        Task<List<Room>> GetFreeRoomsForTimeslot(string timeslotId, long numberOfStudents);
        Task<bool> CheckRoomTimeCollision(string roomId, string timeslotId);
    }
}