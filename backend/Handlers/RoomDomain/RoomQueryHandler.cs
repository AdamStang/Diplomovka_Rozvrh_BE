using backend.Database;
using backend.Extensions;
using backend.Models.LessonDomain;
using backend.Models.RoomDomain;
using backend.Models.TeacherDomain;
using backend.Repositories.RoomDomain;
using backend.Services;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace backend.Handlers.RoomDomain
{
    public class RoomQueryHandler : IRoomQueryHandler
    {
        private readonly IRoomRepository _roomRepository;

        public RoomQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<List<Room>> GetAllRooms()
        {
            //var rooms = new List<Room>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT DISTINCT ?id ?name ?type ?capacity " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @type_prop ?type. " +
                "       ?id @capacity_prop ?capacity. " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(RoomConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(RoomConstants.NAME_PROP));
            queryString.SetUri("type_prop", HelperService.CreateUri(RoomConstants.TYPE_PROP));
            queryString.SetUri("capacity_prop", HelperService.CreateUri(RoomConstants.CAPACITY_PROP));

            object results = await _roomRepository.GetAsync(queryString.ToString());

            var rooms = new List<Room>();

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var room = new Room();
                    room.Id = result.Value("id").ToString().GetUriValue();
                    room.Name = result.Value("name").ToString();
                    room.Capacity = result.Value("capacity").AsValuedNode().AsInteger();
                    room.Type = result.Value("type").ToString();

                    rooms.Add(room);
                }
            }

            return rooms;
        }

        public async Task<long> GetRoomsCount()
        {
            var count = 0l;
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT (COUNT(DISTINCT ?id) as ?count) " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(RoomConstants.CLASS_NAME));

            var results = await _roomRepository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                count = set.FirstOrDefault().Value("count").AsValuedNode().AsInteger();
            }
            return count;
        }

        public async Task<List<Room>> GetFreeRoomsForTimeslot(string timeslotId, long numberOfStudents)
        {
            var rooms = new List<Room>();

            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT ?id ?name ?capacity ?type " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @capacity_prop ?capacity. " +
                "       ?id @type_prop ?type. " +
                "       FILTER NOT EXISTS { " +
                "           ?lessonId @room_prop ?id. " +
                "           ?lessonId @timeslot_prop @timeslot_id. " +
                "       } " +
                "   } " +
                "} ";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(RoomConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(RoomConstants.NAME_PROP));
            queryString.SetUri("capacity_prop", HelperService.CreateUri(RoomConstants.CAPACITY_PROP));
            queryString.SetUri("type_prop", HelperService.CreateUri(RoomConstants.TYPE_PROP));
            queryString.SetUri("room_prop", HelperService.CreateUri(LessonConstants.ROOM_PROP));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("timeslot_id", HelperService.CreateUri(timeslotId));

            object results = await _roomRepository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var room = new Room();
                    room.Id = result.Value("id").ToString().GetUriValue();
                    room.Name = result.Value("name").ToString();
                    room.Capacity = result.Value("capacity").AsValuedNode().AsInteger();
                    room.Type = result.Value("type").ToString();

                    if (room.Capacity >= numberOfStudents)
                        rooms.Add(room);
                }
            }

            return rooms;
        }

        public async Task<bool> CheckRoomTimeCollision(string roomId, string timeslotId)
        {
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "ASK FROM NAMED @graph_name { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @room_prop @room_id. " +
                "       ?id @timeslot_prop @timeslot_id. " +
                "   }" +
                "} ";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(LessonConstants.CLASS_NAME));
            queryString.SetUri("room_prop", HelperService.CreateUri(LessonConstants.ROOM_PROP));
            queryString.SetUri("room_id", HelperService.CreateUri(roomId));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("timeslot_id", HelperService.CreateUri(timeslotId));

            object results = await _roomRepository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                return set.Result;
            }
            return false;
        }
    }
}
