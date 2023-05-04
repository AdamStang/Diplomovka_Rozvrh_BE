using backend.Database;
using Microsoft.AspNetCore.Mvc;
using VDS.RDF.Parsing;
using VDS.RDF;
using VDS.RDF.Query;
using backend.Services;
using backend.Models;
using VDS.RDF.Nodes;
using backend.Extensions;
using backend.Models.RoomDomain;
using backend.Handlers.RoomDomain;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomQueryHandler _queryHandler;
        private readonly IRoomCommandHandler _commandHandler;

        public RoomsController(IRoomQueryHandler queryHandler, IRoomCommandHandler commandHandler)
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpGet("getAllRooms")]
        public async Task<List<Room>> GetAllRooms()
        {
            return await _queryHandler.GetAllRooms();
        }

        [HttpGet("getRoomsCount")]
        public async Task<long> GetRoomsCount()
        {
            return await _queryHandler.GetRoomsCount();
        }

        [HttpPost("createRoom")]
        public async Task CreateRoom(Room room)
        {
            await _commandHandler.CreateRoom(room);
        }

        [HttpDelete("deleteRoom")]
        public async Task DeleteRoom(string roomId)
        {
            await _commandHandler.DeleteRoom(roomId);
        }

        [HttpGet("getFreeRoomsForTimeslot")]
        public async Task<List<Room>> GetFreeRoomsForTimeslot(string timeslotId, long numberOfStudents)
        {
            return await _queryHandler.GetFreeRoomsForTimeslot(timeslotId, numberOfStudents);
        }

        [HttpGet("checkRoomTimeCollision")]
        public async Task<bool> CheckRoomTimeCollision(string roomId, string timeslotId)
        {
            return await _queryHandler.CheckRoomTimeCollision(roomId, timeslotId);
        }
    }
}
