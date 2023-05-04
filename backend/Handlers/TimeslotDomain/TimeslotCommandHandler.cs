using backend.Handlers.TeacherDomain;
using backend.Models.TeacherDomain;
using backend.Repositories.TimeslotDomain;
using backend.Services;
using VDS.RDF.Parsing;
using VDS.RDF;
using VDS.RDF.Query;
using backend.Models.TimeslotDomain;
using Newtonsoft.Json;

namespace backend.Handlers.TimeslotDomain
{
    public class TimeslotCommandHandler : ITimeslotCommandHandler
    {
        private readonly ITimeslotRepository _repository;

        public TimeslotCommandHandler(ITimeslotRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateTimeslots(List<Timeslot> timeslots)
        {
            await DeleteTimeslots();

            var timeslotTriples = new List<Triple>();

            timeslots.ForEach(timeslot =>
            {
                var id = timeslot.CreateTimeslotId();

                INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(id));
                INode predicate = _repository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
                INode obj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TimeslotConstants.CLASS_NAME));
                timeslotTriples.Add(new Triple(subject, predicate, obj));

                INode predicateIsOnDay = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TimeslotConstants.DAY_PROP));
                INode objDay = _repository.GetGraph().CreateLiteralNode(timeslot.Day);
                timeslotTriples.Add(new Triple(subject, predicateIsOnDay, objDay));

                INode predicatehasStartTime = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TimeslotConstants.START_TIME_PROP));
                INode objStartTime = _repository.GetGraph().CreateLiteralNode(TimeSpan.Parse(timeslot.StartTime).ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeTime));
                timeslotTriples.Add(new Triple(subject, predicatehasStartTime, objStartTime));

                INode predicatehasEndTime = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TimeslotConstants.END_TIME_PROP));
                INode objEndTime = _repository.GetGraph().CreateLiteralNode(TimeSpan.Parse(timeslot.EndTime).ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeTime));
                timeslotTriples.Add(new Triple(subject, predicatehasEndTime, objEndTime));

            });

            await _repository.CreateAsync(timeslotTriples);
        }

        public async Task DeleteTimeslots()
        {
            var queryString = new SparqlParameterizedString();

            queryString.CommandText = "DELETE { GRAPH ?g { ?s a @class_name . ?s ?p ?o . } } WHERE { GRAPH ?g { ?s a @class_name . ?s ?p ?o . } }";
            queryString.SetUri("class_name", HelperService.CreateUri(TimeslotConstants.CLASS_NAME));

            await _repository.DeleteAsync(queryString.ToString());
        }
    }
}
