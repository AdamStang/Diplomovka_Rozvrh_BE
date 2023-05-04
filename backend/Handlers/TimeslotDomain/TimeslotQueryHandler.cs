using backend.Database;
using backend.Extensions;
using backend.Handlers.TeacherDomain;
using backend.Models.TeacherDomain;
using backend.Models.TimeslotDomain;
using backend.Repositories.TimeslotDomain;
using backend.Services;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace backend.Handlers.TimeslotDomain
{
    public class TimeslotQueryHandler : ITimeslotQueryHandler
    {
        private readonly ITimeslotRepository _repository;

        public TimeslotQueryHandler(ITimeslotRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Timeslot>> GetAllTimeslots()
        {
            var timeslots = new List<Timeslot>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT DISTINCT ?id ?day ?startTime ?endTime " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @day_prop ?day. " +
                "       ?id @start_time_prop ?startTime. " +
                "       ?id @end_time_prop ?endTime. " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(TimeslotConstants.CLASS_NAME));
            queryString.SetUri("day_prop", HelperService.CreateUri(TimeslotConstants.DAY_PROP));
            queryString.SetUri("start_time_prop", HelperService.CreateUri(TimeslotConstants.START_TIME_PROP));
            queryString.SetUri("end_time_prop", HelperService.CreateUri(TimeslotConstants.END_TIME_PROP));

            object results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var timeslot = new Timeslot();
                    timeslot.Id = result.Value("id").ToString().GetUriValue();
                    timeslot.Day = result.Value("day").ToString();
                    timeslot.StartTime = result.Value("startTime").AsValuedNode().AsString();
                    timeslot.EndTime = result.Value("endTime").AsValuedNode().AsString();

                    timeslots.Add(timeslot);
                }
            }

            return timeslots;
        }
    }
}
