using backend.Database;
using backend.Extensions;
using backend.Models.LessonDomain;
using backend.Models.RoomDomain;
using backend.Models.SchoolSubjectDomain;
using backend.Models.TeacherDomain;
using backend.Models.TimeslotDomain;
using backend.Repositories.SchoolSubjectDomain;
using backend.Services;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace backend.Handlers.SchoolSubjectDomain
{
    public class SchoolSubjectQueryHandler : ISchoolSubjectQueryHandler
    {
        private readonly ISchoolSubjectRepository _repository;

        public SchoolSubjectQueryHandler(ISchoolSubjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SchoolSubject>> GetAllSchoolSubjects()
        {
            var schoolSubjects = new List<SchoolSubject>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT DISTINCT ?id ?name ?abbr ?numOfLectures ?numOfPractice ?numOfGroups ?teacherId ?teacherName ?teacherDegree " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @abbr_prop ?abbr. " +
                "       ?id @lectures_num_prop ?numOfLectures. " +
                "       ?id @practice_num_prop ?numOfPractice. " +
                "       ?id @groups_prop ?numOfGroups. " +
                "       OPTIONAL { " +
                "           ?id @teacher_prop ?teacherId. " +
                "           ?teacherId @teacher_name_prop ?teacherName. " +
                "           ?teacherId @teacher_degree_prop ?teacherDegree. " +
                "       } " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(SchoolSubjectConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(SchoolSubjectConstants.NAME_PROP));
            queryString.SetUri("abbr_prop", HelperService.CreateUri(SchoolSubjectConstants.ABBR_PROP));
            queryString.SetUri("lectures_num_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_LECTURES_PROP));
            queryString.SetUri("practice_num_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_PRACTICE_PROP));
            queryString.SetUri("groups_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_GROUPS_PROP));
            queryString.SetUri("teacher_prop", HelperService.CreateUri(SchoolSubjectConstants.TEACHER_PROP));
            queryString.SetUri("teacher_name_prop", HelperService.CreateUri(TeacherConstants.NAME_PROP));
            queryString.SetUri("teacher_degree_prop", HelperService.CreateUri(TeacherConstants.DEGREE_PROP));

            object results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var schoolSubject = new SchoolSubject();
                    schoolSubject.Id = result.Value("id").ToString().GetUriValue();
                    schoolSubject.Name = result.Value("name").ToString();
                    schoolSubject.Abbr = result.Value("abbr").ToString();
                    schoolSubject.LecturesPerWeek = result.Value("numOfLectures").AsValuedNode().AsInteger();
                    schoolSubject.PracticePerWeek = result.Value("numOfPractice").AsValuedNode().AsInteger();
                    schoolSubject.NumberOfGroups = result.Value("numOfGroups").AsValuedNode().AsInteger();

                    if (result.Value("teacherId") != null)
                    {
                        var teacher = new Teacher();
                        teacher.Id = result.Value("teacherId").ToString().GetUriValue();
                        teacher.Name = result.Value("teacherName").ToString();
                        teacher.Degree = result.Value("teacherDegree").ToString();
                        schoolSubject.Teacher = teacher;
                    }

                    schoolSubjects.Add(schoolSubject);
                }
            }

            return schoolSubjects;
        }

        public async Task<long> GetSchoolSubjectsCount()
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
            queryString.SetUri("class_name", HelperService.CreateUri(SchoolSubjectConstants.CLASS_NAME));

            var results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                count = set.FirstOrDefault().Value("count").AsValuedNode().AsInteger();
            }
            return count;
        }

        public async Task<List<Lesson>> GetSubjectsLessons(string subjectId)
        {
            var lessons = new List<Lesson>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT ?lessonId ?lessonName ?lessonType ?teacherId ?teacherName ?teacherDegree ?lessonGroup ?roomId ?roomName ?roomCapacity ?roomType ?timeslotId ?timeslotDay ?timeslotStart ?timeslotEnd " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       @id a @subject_class. " +
                "       @id @lesson_prop ?lessonId. " +
                "       ?lessonId @lesson_name_prop ?lessonName. " +
                "       ?lessonId @lesson_type_prop ?lessonType. " +
                "       OPTIONAL { " +
                "           ?lessonId @lesson_group_prop ?lessonGroup. " +
                "       } " +
                "       OPTIONAL { " +
                "           ?lessonId @teacher_prop ?teacherId. " +
                "           ?teacherId @teacher_name_prop ?teacherName. " +
                "           ?teacherId @teacher_degree_prop ?teacherDegree. " +
                "       } " +
                "       OPTIONAL { " +
                "           ?lessonId @room_prop ?roomId. " +
                "           ?roomId @room_name_prop ?roomName. " +
                "           ?roomId @room_capacity_prop ?roomCapacity. " +
                "           ?roomId @room_type_prop ?roomType. " +
                "       } " +
                "       OPTIONAL { " +
                "           ?lessonId @timeslot_prop ?timeslotId. " +
                "           ?timeslotId @timeslot_day_prop ?timeslotDay. " +
                "           ?timeslotId @timeslot_start_prop ?timeslotStart. " +
                "           ?timeslotId @timeslot_end_prop ?timeslotEnd. " +
                "       } " +
                "   } " +
                "} ";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("id", HelperService.CreateUri(subjectId));
            queryString.SetUri("subject_class", HelperService.CreateUri(SchoolSubjectConstants.CLASS_NAME));
            queryString.SetUri("lesson_prop", HelperService.CreateUri(SchoolSubjectConstants.LESSONS_PROP));
            queryString.SetUri("lesson_name_prop", HelperService.CreateUri(LessonConstants.NAME_PROP));
            queryString.SetUri("lesson_type_prop", HelperService.CreateUri(LessonConstants.TYPE_PROP));
            queryString.SetUri("lesson_group_prop", HelperService.CreateUri(LessonConstants.GROUP_PROP));
            queryString.SetUri("teacher_prop", HelperService.CreateUri(LessonConstants.TEACHER_PROP));
            queryString.SetUri("teacher_name_prop", HelperService.CreateUri(TeacherConstants.NAME_PROP));
            queryString.SetUri("teacher_degree_prop", HelperService.CreateUri(TeacherConstants.DEGREE_PROP));
            queryString.SetUri("room_prop", HelperService.CreateUri(LessonConstants.ROOM_PROP));
            queryString.SetUri("room_name_prop", HelperService.CreateUri(RoomConstants.NAME_PROP));
            queryString.SetUri("room_capacity_prop", HelperService.CreateUri(RoomConstants.CAPACITY_PROP));
            queryString.SetUri("room_type_prop", HelperService.CreateUri(RoomConstants.TYPE_PROP));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("timeslot_day_prop", HelperService.CreateUri(TimeslotConstants.DAY_PROP));
            queryString.SetUri("timeslot_start_prop", HelperService.CreateUri(TimeslotConstants.START_TIME_PROP));
            queryString.SetUri("timeslot_end_prop", HelperService.CreateUri(TimeslotConstants.END_TIME_PROP));

            object results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var lesson = new Lesson();
                    lesson.Id = result.Value("lessonId").ToString().GetUriValue();
                    lesson.Name = result.Value("lessonName").ToString();
                    lesson.Type = result.Value("lessonType").ToString();
                    if (result.Value("lessonGroup") != null) 
                        lesson.Group = result.Value("lessonGroup").AsValuedNode().AsInteger();

                    if (result.Value("teacherId") != null)
                    {
                        var teacher = new Teacher();
                        teacher.Id = result.Value("teacherId").ToString().GetUriValue();
                        teacher.Name = result.Value("teacherName").ToString();
                        teacher.Degree = result.Value("teacherDegree").ToString();
                        lesson.Teacher = teacher;
                    }

                    if (result.Value("roomId") != null)
                    {
                        var room = new Room();
                        room.Id = result.Value("roomId").ToString().GetUriValue();
                        room.Name = result.Value("roomName").ToString();
                        room.Capacity = result.Value("roomCapacity").AsValuedNode().AsInteger();
                        room.Type = result.Value("roomType").ToString();
                        lesson.Room = room;
                    }

                    if (result.Value("timeslotId") != null)
                    {
                        var timeslot = new Timeslot();
                        timeslot.Id = result.Value("timeslotId").ToString().GetUriValue();
                        timeslot.Day = result.Value("timeslotDay").ToString();
                        timeslot.StartTime = result.Value("timeslotStart").AsValuedNode().AsString();
                        timeslot.EndTime = result.Value("timeslotEnd").AsValuedNode().AsString();
                        lesson.Timeslot = timeslot;
                    }

                    lessons.Add(lesson);
                }
            }

            return lessons;
        }
    }
}
