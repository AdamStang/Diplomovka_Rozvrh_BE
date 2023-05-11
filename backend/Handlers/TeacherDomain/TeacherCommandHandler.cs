using backend.Handlers.RoomDomain;
using backend.Models.RoomDomain;
using backend.Repositories.TeacherDomain;
using backend.Services;
using VDS.RDF.Parsing;
using VDS.RDF;
using backend.Models.TeacherDomain;
using VDS.RDF.Query;
using backend.Models.LessonDomain;
using backend.Models.TimeslotDomain;

namespace backend.Handlers.TeacherDomain
{
    public class TeacherCommandHandler : ITeacherCommandHandler
    {
        private readonly ITeacherRepository _repository;

        public TeacherCommandHandler(ITeacherRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateTeacher(Teacher teacher)
        {
            var id = teacher.CreateTeacherId();
            var teacherTriples = new List<Triple>();

            INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(id));
            INode predicate = _repository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
            INode obj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TeacherConstants.CLASS_NAME));
            teacherTriples.Add(new Triple(subject, predicate, obj));

            INode predicateHasName = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TeacherConstants.NAME_PROP));
            INode objName = _repository.GetGraph().CreateLiteralNode(teacher.Name);
            teacherTriples.Add(new Triple(subject, predicateHasName, objName));

            INode predicateHasDegree = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TeacherConstants.DEGREE_PROP));
            INode objDegree = _repository.GetGraph().CreateLiteralNode(teacher.Degree);
            teacherTriples.Add(new Triple(subject, predicateHasDegree, objDegree));

            if (teacher.Department != null)
            {
                INode departmentPredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TeacherConstants.DEPARTMENT_PROP));
                INode objDepartment = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(teacher.Department.Id));
                teacherTriples.Add(new Triple(subject, departmentPredicate, objDepartment));
            }

            await _repository.CreateAsync(teacherTriples);
        }

        public async Task DeleteTeacher(string teacherId)
        {
            var queryString = new SparqlParameterizedString();

            queryString.CommandText = "DELETE { GRAPH ?g { @id ?p1 ?o. ?s ?p2 @id. ?s @timeslot_prop ?t. ?s @room_prop ?r. } } WHERE { Graph ?g { { @id ?p1 ?o. } UNION { ?s ?p2 @id. OPTIONAL { ?s @timeslot_prop ?t. ?s @room_prop ?r. } } } }";
            queryString.SetUri("id", HelperService.CreateUri(teacherId));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("room_prop", HelperService.CreateUri(LessonConstants.ROOM_PROP));

            await _repository.DeleteAsync(queryString.ToString());
        }

        public async Task UpdateTeacher(Teacher teacher)
        {
            var teacherTriples = new List<Triple>();

            INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(teacher.Id));
            //INode predicate = _repository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
            //INode obj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TeacherConstants.CLASS_NAME));
            //teacherTriples.Add(new Triple(subject, predicate, obj));

            if (teacher.Department?.Id != null)
            {
                INode predicateBelongsToDepartment = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(TeacherConstants.DEPARTMENT_PROP));
                INode objDepartment = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(teacher.Department.Id));
                teacherTriples.Add(new Triple(subject, predicateBelongsToDepartment, objDepartment));
            }

            //await _repository.UpdateAsync(teacherTriples);
        }

        public async Task MoveTeachersToNewDepartment(MoveTeachersCommand teachersCommand)
        {
            var newTeacherTriples = new List<Triple>();
            var previousTeacherTriples = new List<Triple>();

            foreach (var teacher in teachersCommand.TeacherIds)
            {
                INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(teacher));
                INode predicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TeacherConstants.DEPARTMENT_PROP));
                INode obj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(teachersCommand.NewDepartmentId));
                newTeacherTriples.Add(new Triple(subject, predicate, obj));

                if (teachersCommand.PreviousDepartmentId != null)
                {
                    INode previousObj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(teachersCommand.PreviousDepartmentId));
                    previousTeacherTriples.Add(new Triple(subject, predicate, previousObj));
                }
            }

            await _repository.UpdateAsync(newTeacherTriples, previousTeacherTriples);
        }

        public async Task UpdateTimeConstraintsForTeacher(UpdateTimeConstraintsCommand constraintsCommand)
        {
            await DeleteTimeslotConstraints(constraintsCommand.TeacherId);

            var teacherTriples = new List<Triple>();

            INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(constraintsCommand.TeacherId));

            foreach (var timeslotId in constraintsCommand.TimeslotsIds)
            {
                INode predicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(TeacherConstants.CONSTRAINTS_PROP));
                INode obj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(timeslotId));
                teacherTriples.Add(new Triple(subject, predicate, obj));
            }

            await _repository.CreateAsync(teacherTriples);
        }

        public async Task DeleteTimeslotConstraints(string teacherId)
        {
            var queryString = new SparqlParameterizedString();

            queryString.CommandText = "DELETE { GRAPH ?g { @teacher_id @constraint_prop ?timeslot. } } WHERE { GRAPH ?g { @teacher_id @constraint_prop ?timeslot. } }";
            queryString.SetUri("teacher_id", HelperService.CreateUri(teacherId));
            queryString.SetUri("constraint_prop", HelperService.CreateUri(TeacherConstants.CONSTRAINTS_PROP));

            await _repository.DeleteAsync(queryString.ToString());
        }
    }
}
