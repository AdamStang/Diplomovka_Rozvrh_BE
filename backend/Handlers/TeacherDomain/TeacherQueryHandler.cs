using backend.Database;
using backend.Extensions;
using backend.Models.DepartmentDomain;
using backend.Models.LessonDomain;
using backend.Models.TeacherDomain;
using backend.Repositories.TeacherDomain;
using backend.Services;
using VDS.RDF;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace backend.Handlers.TeacherDomain
{
    public class TeacherQueryHandler : ITeacherQueryHandler
    {
        private readonly ITeacherRepository _repository;

        public TeacherQueryHandler(ITeacherRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Teacher>> GetAllTeachers()
        {
            var teachers = new List<Teacher>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT DISTINCT ?id ?name ?degree ?departmentId ?departmentName ?departmentAbbr " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @degree_prop ?degree. " +
                "       OPTIONAL { " +
                "           ?id @department_prop ?departmentId. " +
                "           ?departmentId @department_name_prop ?departmentName. " +
                "           ?departmentId @department_abbr_prop ?departmentAbbr. " +
                "       } " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(TeacherConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(TeacherConstants.NAME_PROP));
            queryString.SetUri("degree_prop", HelperService.CreateUri(TeacherConstants.DEGREE_PROP));
            queryString.SetUri("department_prop", HelperService.CreateUri(TeacherConstants.DEPARTMENT_PROP));
            queryString.SetUri("department_name_prop", HelperService.CreateUri(DepartmentConstants.NAME_PROP));
            queryString.SetUri("department_abbr_prop", HelperService.CreateUri(DepartmentConstants.ABBR_PROP));

            object results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var teacher = new Teacher();
                    teacher.Id = result.Value("id").ToString().GetUriValue();
                    teacher.Name = result.Value("name").ToString();
                    teacher.Degree = result.Value("degree").ToString();
                    //teacher.TimeConstraints = await GetTimeslotConstraints(teacher.Id);

                    if (result.Value("departmentId") != null)
                    {
                        var department = new Department();
                        department.Id = result.Value("departmentId").ToString().GetUriValue();
                        department.Name = result.Value("departmentName").ToString().GetUriValue();
                        department.Abbr = result.Value("departmentAbbr").ToString().GetUriValue();
                        teacher.Department = department;
                    }

                    teachers.Add(teacher);
                }
            }

            return teachers;
        }

        public async Task<long> GetTeachersCount()
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
            queryString.SetUri("class_name", HelperService.CreateUri(TeacherConstants.CLASS_NAME));

            var results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                count = set.FirstOrDefault().Value("count").AsValuedNode().AsInteger();
            }
            return count;
        }

        public async Task<List<Teacher>> GetTeachersByDepartment(string departmentId)
        {
            var teachers = new List<Teacher>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT ?id ?name ?degree " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @degree_prop ?degree. " +
                "       ?id @department_prop @department_id. " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(TeacherConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(TeacherConstants.NAME_PROP));
            queryString.SetUri("degree_prop", HelperService.CreateUri(TeacherConstants.DEGREE_PROP));
            queryString.SetUri("department_prop", HelperService.CreateUri(TeacherConstants.DEPARTMENT_PROP));
            queryString.SetUri("department_id", HelperService.CreateUri(departmentId));

            object results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var teacher = new Teacher();
                    teacher.Id = result.Value("id").ToString().GetUriValue();
                    teacher.Name = result.Value("name").ToString();
                    teacher.Degree = result.Value("degree").ToString();

                    teachers.Add(teacher);
                }
            }

            return teachers;
        }

        public async Task<bool> CheckTeacherTimeCollision(string teacherId, string timeslotId)
        {
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "ASK FROM NAMED @graph_name { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @teacher_prop @teacher_id. " +
                "       ?id @timeslot_prop @timeslot_id. " +
                "   }" +
                "} ";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(LessonConstants.CLASS_NAME));
            queryString.SetUri("teacher_prop", HelperService.CreateUri(LessonConstants.TEACHER_PROP));
            queryString.SetUri("teacher_id", HelperService.CreateUri(teacherId));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("timeslot_id", HelperService.CreateUri(timeslotId));

            object results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                return set.Result;
            }
            return false;
        }

        public async Task<List<string>> GetTimeslotConstraints(string teacherId)
        {
            var timeslotIds = new List<string>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT ?timeslotId  " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       @teacher_id @constraint_prop ?timeslotId. " + 
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("teacher_id", HelperService.CreateUri(teacherId));
            queryString.SetUri("constraint_prop", HelperService.CreateUri(TeacherConstants.CONSTRAINTS_PROP));

            object results = await _repository.GetAsync(queryString.ToString());


            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    timeslotIds.Add(result.Value("timeslotId").ToString().GetUriValue());
                }
            }

            return timeslotIds;
        }

        public async Task<bool> CheckTeacherTimeslotConstraint(string teacherId, string timeslotId)
        {
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "ASK FROM NAMED @graph_name { " +
                "   GRAPH ?g { " +
                "       @teacher_id a @class_name. " +
                "       @teacher_id @constraint_prop @timeslot_id. " +
                "   }" +
                "} ";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(TeacherConstants.CLASS_NAME));
            queryString.SetUri("teacher_id", HelperService.CreateUri(teacherId));
            queryString.SetUri("constraint_prop", HelperService.CreateUri(TeacherConstants.CONSTRAINTS_PROP));
            queryString.SetUri("timeslot_id", HelperService.CreateUri(timeslotId));

            object results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                return set.Result;
            }
            return false;
        } 
    }
}
