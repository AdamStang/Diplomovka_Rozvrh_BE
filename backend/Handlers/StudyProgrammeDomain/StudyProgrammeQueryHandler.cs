using backend.Database;
using backend.Extensions;
using backend.Handlers.SchoolSubjectDomain;
using backend.Models;
using backend.Models.StudyProgrammeDomain;
using backend.Repositories.StudyProgrammeDomain;
using backend.Services;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace backend.Handlers.StudyProgrammeDomain
{
    public class StudyProgrammeQueryHandler : IStudyProgrammeQueryHandler
    {
        private readonly IStudyProgrammeRepository _repository;

        public StudyProgrammeQueryHandler(IStudyProgrammeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StudyProgramme>> GetAllStudyProgrammes()
        {
            var studyProgrammes = new List<StudyProgramme>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT DISTINCT ?id ?name ?abbr ?grade ?numOfStudents ?subParallels ?numOfGroups " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @abbr_prop ?abbr. " +
                "       ?id @grade_prop ?grade. " +
                "       ?id @num_of_studs_prop ?numOfStudents. " +
                "       ?id @num_of_groups_prop ?numOfGroups. " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(StudyProgrammeConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(StudyProgrammeConstants.NAME_PROP));
            queryString.SetUri("abbr_prop", HelperService.CreateUri(StudyProgrammeConstants.ABBR_PROP));
            queryString.SetUri("grade_prop", HelperService.CreateUri(StudyProgrammeConstants.GRADE_PROP));
            queryString.SetUri("num_of_studs_prop", HelperService.CreateUri(StudyProgrammeConstants.NUM_OF_STUDS_PROP));
            queryString.SetUri("num_of_groups_prop", HelperService.CreateUri(StudyProgrammeConstants.NUM_OF_GROUPS_PROP));

            object results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var studyProgramme = new StudyProgramme();
                    studyProgramme.Id = result.Value("id").ToString().GetUriValue();
                    studyProgramme.Name = result.Value("name").ToString();
                    studyProgramme.Abbr = result.Value("abbr").ToString();
                    studyProgramme.Grade = result.Value("grade").AsValuedNode().AsInteger();
                    studyProgramme.NumberOfStudents = result.Value("numOfStudents").AsValuedNode().AsInteger();
                    studyProgramme.NumberOfGroups = result.Value("numOfGroups").AsValuedNode().AsInteger();

                    studyProgrammes.Add(studyProgramme);
                }
            }

            return studyProgrammes;
        }
    }
}
