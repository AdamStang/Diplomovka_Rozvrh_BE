using backend.Database;
using backend.Extensions;
using backend.Models.DepartmentDomain;
using backend.Models.TeacherDomain;
using backend.Repositories.DepartmentDomain;
using backend.Services;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace backend.Handlers.DepartmentDomain
{
    public class DepartmentQueryHandler : IDepartmentQueryHandler
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentQueryHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            var departments = new List<Department>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT DISTINCT ?id ?name ?abbr " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @abbr_prop ?abbr. " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(DepartmentConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(DepartmentConstants.NAME_PROP));
            queryString.SetUri("abbr_prop", HelperService.CreateUri(DepartmentConstants.ABBR_PROP));

            object results = await _departmentRepository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var department = new Department();
                    department.Id = result.Value("id").ToString().GetUriValue();
                    department.Name = result.Value("name").ToString();
                    department.Abbr = result.Value("abbr").ToString();

                    departments.Add(department);
                }
            }

            return departments;
        }

        public async Task<long> GetDepartmentsCount()
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
            queryString.SetUri("class_name", HelperService.CreateUri(DepartmentConstants.CLASS_NAME));

            var results = await _departmentRepository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                count = set.FirstOrDefault().Value("count").AsValuedNode().AsInteger();
            }
            return count;
        }
    }
}
