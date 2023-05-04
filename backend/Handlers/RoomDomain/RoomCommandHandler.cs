using backend.Repositories.RoomDomain;
using backend.Services;
using VDS.RDF.Parsing;
using VDS.RDF;
using VDS.RDF.Query;
using backend.Models.RoomDomain;
using backend.Models.LessonDomain;

namespace backend.Handlers.RoomDomain
{
    public class RoomCommandHandler : IRoomCommandHandler
    {
        private readonly IRoomRepository _roomRepository;
        //private readonly IHashing _hashing;

        public RoomCommandHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
            //_hashing = hashing;
        }

        public async Task CreateRoom(Room room)
        {
            var id = room.CreateRoomId();
            var roomTriples = new List<Triple>();

            INode subject = _roomRepository.GetGraph().CreateUriNode(HelperService.CreateUri(id));
            INode predicate = _roomRepository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
            INode obj = _roomRepository.GetGraph().GetUriNode(HelperService.CreateUri(RoomConstants.CLASS_NAME));
            roomTriples.Add(new Triple(subject, predicate, obj));

            INode predicateHasName = _roomRepository.GetGraph().GetUriNode(HelperService.CreateUri(RoomConstants.NAME_PROP));
            INode objName = _roomRepository.GetGraph().CreateLiteralNode(room.Name);
            roomTriples.Add(new Triple(subject, predicateHasName, objName));

            INode predicateIsOfType = _roomRepository.GetGraph().GetUriNode(HelperService.CreateUri(RoomConstants.TYPE_PROP));
            INode objType = _roomRepository.GetGraph().CreateLiteralNode(room.Type);
            roomTriples.Add(new Triple(subject, predicateIsOfType, objType));

            INode predicateHasCapacity = _roomRepository.GetGraph().GetUriNode(HelperService.CreateUri(RoomConstants.CAPACITY_PROP));
            INode objCapacity = _roomRepository.GetGraph().CreateLiteralNode(room.Capacity.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
            roomTriples.Add(new Triple(subject, predicateHasCapacity, objCapacity));

            await _roomRepository.CreateAsync(roomTriples);
        }

        public async Task DeleteRoom(string roomId)
        {
            var queryString = new SparqlParameterizedString();

            queryString.CommandText = "DELETE { GRAPH ?g { @id ?p1 ?o. ?s ?p2 @id. ?s @timeslot_prop ?t. } } WHERE { Graph ?g { @id ?p1 ?o. OPTIONAL { ?s ?p2 @id. } OPTIONAL { ?s @timeslot_prop ?t. } } }";
            queryString.SetUri("id", HelperService.CreateUri(roomId));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));

            await _roomRepository.DeleteAsync(queryString.ToString());
        }
    }
}
