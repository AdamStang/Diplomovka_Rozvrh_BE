using backend.Database;
using backend.Extensions;
using backend.Models.LessonDomain;
using backend.Models.RoomDomain;
using backend.Models.SchoolSubjectDomain;
using backend.Models.StudyProgrammeDomain;
using backend.Models.TeacherDomain;
using backend.Models.TimeslotDomain;
using backend.Repositories.LessonDomain;
using backend.Services;
using VDS.RDF.Nodes;
using VDS.RDF.Query;

namespace backend.Handlers.LessonDomain
{
    public class LessonQueryHandler : ILessonQueryHandler
    {
        private readonly ILessonRepository _repository;

        public LessonQueryHandler(ILessonRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Lesson>> GetLessonsInRoom(string roomId)
        {
            var lessons = new List<Lesson>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT ?id ?name ?type ?group ?subjectId ?subjectName ?subjectAbbr ?subjectNumOfGroups ?timeslotId ?start ?end ?day ?teacherId ?teacherName ?studyProgId ?studyProgName ?studyProgAbbr ?studyProgGrade " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @type_prop ?type. " +
                "       OPTIONAL {  " +
                "           ?id @group_prop ?group. " +
                "       } " +
                "       ?id @room_prop @room_id . " +
                "       ?id @subject_prop ?subjectId. " +
                "       ?subjectId @name_prop ?subjectName. " +
                "       ?subjectId @abbr_prop ?subjectAbbr. " +
                "       ?subjectId @subject_group_prop ?subjectNumOfGroups. " +
                "       ?id @timeslot_prop ?timeslotId. " +
                "       ?timeslotId @start_time_prop ?start. " +
                "       ?timeslotId @end_time_prop ?end. " +
                "       ?timeslotId @day_prop ?day. " +
                "       OPTIONAL { " +
                "           ?id @teacher_prop ?teacherId. " +
                "           ?teacherId @teacher_name_prop ?teacherName. " +
                "       } " +
                "       OPTIONAL { " +
                "           ?subjectId @study_prog_prop ?studyProgId. " +
                "           ?studyProgId @study_prog_name_prop ?studyProgName. " +
                "           ?studyProgId @study_prog_abbr_prop ?studyProgAbbr. " +
                "           ?studyProgId @study_prog_grade_prop ?studyProgGrade. " +
                "       } " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(LessonConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(LessonConstants.NAME_PROP));
            queryString.SetUri("type_prop", HelperService.CreateUri(LessonConstants.TYPE_PROP));
            queryString.SetUri("group_prop", HelperService.CreateUri(LessonConstants.GROUP_PROP));
            queryString.SetUri("room_prop", HelperService.CreateUri(LessonConstants.ROOM_PROP));
            queryString.SetUri("room_id", HelperService.CreateUri(roomId));
            queryString.SetUri("subject_prop", HelperService.CreateUri(LessonConstants.SUBJECT_PROP));
            queryString.SetUri("subject_group_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_GROUPS_PROP));
            queryString.SetUri("name_prop", HelperService.CreateUri(SchoolSubjectConstants.NAME_PROP));
            queryString.SetUri("abbr_prop", HelperService.CreateUri(SchoolSubjectConstants.ABBR_PROP));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("start_time_prop", HelperService.CreateUri(TimeslotConstants.START_TIME_PROP));
            queryString.SetUri("end_time_prop", HelperService.CreateUri(TimeslotConstants.END_TIME_PROP));
            queryString.SetUri("day_prop", HelperService.CreateUri(TimeslotConstants.DAY_PROP));
            queryString.SetUri("teacher_prop", HelperService.CreateUri(LessonConstants.TEACHER_PROP));
            queryString.SetUri("teacher_name_prop", HelperService.CreateUri(TeacherConstants.NAME_PROP));
            queryString.SetUri("study_prog_prop", HelperService.CreateUri(SchoolSubjectConstants.STUDY_PROG_PROP));
            queryString.SetUri("study_prog_name_prop", HelperService.CreateUri(StudyProgrammeConstants.NAME_PROP));
            queryString.SetUri("study_prog_abbr_prop", HelperService.CreateUri(StudyProgrammeConstants.ABBR_PROP));
            queryString.SetUri("study_prog_grade_prop", HelperService.CreateUri(StudyProgrammeConstants.GRADE_PROP));

            var results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var lesson = new Lesson();
                    lesson.Id = result.Value("id").ToString().GetUriValue();
                    lesson.Name = result.Value("name").ToString();
                    lesson.Type = result.Value("type").ToString();
                    if (result.Value("group") != null)
                        lesson.Group = result.Value("group").AsValuedNode().AsInteger();

                    var subject = new SchoolSubject();
                    subject.Id = result.Value("subjectId").ToString().GetUriValue();
                    subject.Name = result.Value("subjectName").ToString();
                    subject.Abbr = result.Value("subjectAbbr").ToString();
                    subject.NumberOfGroups = result.Value("subjectNumOfGroups").AsValuedNode().AsInteger();

                    var timeslot = new Timeslot();
                    timeslot.Id = result.Value("timeslotId").ToString().GetUriValue();
                    timeslot.StartTime = result.Value("start").AsValuedNode().AsString();
                    timeslot.EndTime = result.Value("end").AsValuedNode().AsString();
                    timeslot.Day = result.Value("day").ToString();

                    if (result.Value("teacherId") != null)
                    {
                        var teacher = new Teacher();
                        teacher.Id = result.Value("teacherId").ToString().GetUriValue();
                        teacher.Name = result.Value("teacherName").ToString();
                        lesson.Teacher = teacher;
                    }

                    if (result.Value("studyProgId") != null)
                    {
                        var studyProgramme = new StudyProgramme();
                        studyProgramme.Id = result.Value("studyProgId").ToString().GetUriValue();
                        studyProgramme.Name = result.Value("studyProgName").ToString();
                        studyProgramme.Abbr = result.Value("studyProgAbbr").ToString();
                        studyProgramme.Grade = result.Value("studyProgGrade").AsValuedNode().AsInteger();
                        subject.StudyProgramme = studyProgramme;
                    }

                    lesson.SchoolSubject= subject;
                    lesson.Timeslot = timeslot;

                    lessons.Add(lesson);
                }
            }
            return lessons;
        }

        public async Task<List<Lesson>> GetLessonsForTeacher(string teacherId)
        {
            var lessons = new List<Lesson>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT ?id ?name ?type ?group ?subjectId ?subjectName ?subjectAbbr ?subjectNumOfGroups ?timeslotId ?start ?end ?day ?roomId ?roomName ?studyProgId ?studyProgName ?studyProgAbbr ?studyProgGrade " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @type_prop ?type. " +
                "       OPTIONAL { " +
                "           ?id @group_prop ?group. " +
                "       } " +
                "       ?id @teacher_prop @teacher_id. " +
                "       ?id @subject_prop ?subjectId. " +
                "       ?subjectId @name_prop ?subjectName. " +
                "       ?subjectId @abbr_prop ?subjectAbbr. " +
                "       ?subjectId @subject_group_prop ?subjectNumOfGroups. " +
                "       ?id @timeslot_prop ?timeslotId. " +
                "       ?timeslotId @start_time_prop ?start. " +
                "       ?timeslotId @end_time_prop ?end. " +
                "       ?timeslotId @day_prop ?day. " +
                "       OPTIONAL { " +
                "           ?id @room_prop ?roomId. " +
                "           ?roomId @room_name_prop ?roomName. " +
                "       } " +
                "       OPTIONAL { " +
                "           ?subjectId @study_prog_prop ?studyProgId. " +
                "           ?studyProgId @study_prog_name_prop ?studyProgName. " +
                "           ?studyProgId @study_prog_abbr_prop ?studyProgAbbr. " +
                "           ?studyProgId @study_prog_grade_prop ?studyProgGrade. " +
                "       } " +
                "   } " +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(LessonConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(LessonConstants.NAME_PROP));
            queryString.SetUri("type_prop", HelperService.CreateUri(LessonConstants.TYPE_PROP));
            queryString.SetUri("group_prop", HelperService.CreateUri(LessonConstants.GROUP_PROP));
            queryString.SetUri("teacher_prop", HelperService.CreateUri(LessonConstants.TEACHER_PROP));
            queryString.SetUri("teacher_id", HelperService.CreateUri(teacherId));
            queryString.SetUri("subject_prop", HelperService.CreateUri(LessonConstants.SUBJECT_PROP));
            queryString.SetUri("subject_group_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_GROUPS_PROP));
            queryString.SetUri("name_prop", HelperService.CreateUri(SchoolSubjectConstants.NAME_PROP));
            queryString.SetUri("abbr_prop", HelperService.CreateUri(SchoolSubjectConstants.ABBR_PROP));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("start_time_prop", HelperService.CreateUri(TimeslotConstants.START_TIME_PROP));
            queryString.SetUri("end_time_prop", HelperService.CreateUri(TimeslotConstants.END_TIME_PROP));
            queryString.SetUri("day_prop", HelperService.CreateUri(TimeslotConstants.DAY_PROP));
            queryString.SetUri("room_prop", HelperService.CreateUri(LessonConstants.ROOM_PROP));
            queryString.SetUri("room_name_prop", HelperService.CreateUri(RoomConstants.NAME_PROP));
            queryString.SetUri("study_prog_prop", HelperService.CreateUri(SchoolSubjectConstants.STUDY_PROG_PROP));
            queryString.SetUri("study_prog_name_prop", HelperService.CreateUri(StudyProgrammeConstants.NAME_PROP));
            queryString.SetUri("study_prog_abbr_prop", HelperService.CreateUri(StudyProgrammeConstants.ABBR_PROP));
            queryString.SetUri("study_prog_grade_prop", HelperService.CreateUri(StudyProgrammeConstants.GRADE_PROP));

            var results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var lesson = new Lesson();
                    lesson.Id = result.Value("id").ToString().GetUriValue();
                    lesson.Name = result.Value("name").ToString();
                    lesson.Type = result.Value("type").ToString();
                    if (result.Value("group") != null)
                        lesson.Group = result.Value("group").AsValuedNode().AsInteger();

                    var subject = new SchoolSubject();
                    subject.Id = result.Value("subjectId").ToString().GetUriValue();
                    subject.Name = result.Value("subjectName").ToString();
                    subject.Abbr = result.Value("subjectAbbr").ToString();
                    subject.NumberOfGroups = result.Value("subjectNumOfGroups").AsValuedNode().AsInteger();

                    var timeslot = new Timeslot();
                    timeslot.Id = result.Value("timeslotId").ToString().GetUriValue();
                    timeslot.StartTime = result.Value("start").AsValuedNode().AsString();
                    timeslot.EndTime = result.Value("end").AsValuedNode().AsString();
                    timeslot.Day = result.Value("day").ToString();

                    if (result.Value("roomId") != null)
                    {
                        var room = new Room();
                        room.Id = result.Value("roomId").ToString().GetUriValue();
                        room.Name = result.Value("roomName").ToString();
                        lesson.Room = room;
                    }

                    if (result.Value("studyProgId") != null)
                    {
                        var studyProgramme = new StudyProgramme();
                        studyProgramme.Id = result.Value("studyProgId").ToString().GetUriValue();
                        studyProgramme.Name = result.Value("studyProgName").ToString();
                        studyProgramme.Abbr = result.Value("studyProgAbbr").ToString();
                        studyProgramme.Grade = result.Value("studyProgGrade").AsValuedNode().AsInteger();
                        subject.StudyProgramme = studyProgramme;
                    }

                    lesson.SchoolSubject = subject;
                    lesson.Timeslot = timeslot;

                    lessons.Add(lesson);
                }
            }
            return lessons;
        }

        public async Task<List<Lesson>> GetAssignedLessonsForStudyProgramme(string studyProgrammeId)
        {
            var lessons = new List<Lesson>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT ?id ?type ?name ?group ?numOfStudents ?timeslotId ?start ?end ?day ?subjectId ?subjectName ?subjectAbbr ?subjectNumOfGroups ?roomId ?roomName ?teacherId ?teacherName ?teacherDegree " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @type_prop ?type. " +
                "       ?id @name_prop ?name. " +
                "       OPTIONAL { " +
                "           ?id @num_studs_prop ?numOfStudents. " +
                "       } " +
                "       OPTIONAL { " +
                "           ?id @group_prop ?group. " +
                "       } " +
                "       ?id @subject_prop ?subjectId. " +
                "       ?subjectId @programme_prop @programme_id. " +
                "       ?subjectId @subject_name_prop ?subjectName. " +
                "       ?subjectId @subject_abbr_prop ?subjectAbbr. " +
                "       ?subjectId @subject_group_prop ?subjectNumOfGroups. " +
                "       ?id @teacher_prop ?teacherId. " +
                "       ?teacherId @teacher_name_prop ?teacherName. " +
                "       ?teacherId @teacher_degree_prop ?teacherDegree. " +
                "       ?id @room_prop ?roomId. " +
                "       ?roomId @room_name_prop ?roomName. " +
                "       ?id @timeslot_prop ?timeslotId. " +
                "       ?timeslotId @start_prop ?start. " +
                "       ?timeslotId @end_prop ?end. " +
                "       ?timeslotId @day_prop ?day. " +
                "   }" +
                "}";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(LessonConstants.CLASS_NAME));
            queryString.SetUri("type_prop", HelperService.CreateUri(LessonConstants.TYPE_PROP));
            queryString.SetUri("name_prop", HelperService.CreateUri(LessonConstants.NAME_PROP));
            queryString.SetUri("num_studs_prop", HelperService.CreateUri(LessonConstants.NUM_OF_STUDENTS));
            queryString.SetUri("group_prop", HelperService.CreateUri(LessonConstants.GROUP_PROP));
            queryString.SetUri("subject_prop", HelperService.CreateUri(LessonConstants.SUBJECT_PROP));
            queryString.SetUri("programme_prop", HelperService.CreateUri(SchoolSubjectConstants.STUDY_PROG_PROP));
            queryString.SetUri("programme_id", HelperService.CreateUri(studyProgrammeId));
            queryString.SetUri("subject_name_prop", HelperService.CreateUri(SchoolSubjectConstants.NAME_PROP));
            queryString.SetUri("subject_abbr_prop", HelperService.CreateUri(SchoolSubjectConstants.ABBR_PROP));
            queryString.SetUri("subject_group_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_GROUPS_PROP));
            queryString.SetUri("teacher_prop", HelperService.CreateUri(LessonConstants.TEACHER_PROP));
            queryString.SetUri("teacher_name_prop", HelperService.CreateUri(TeacherConstants.NAME_PROP));
            queryString.SetUri("teacher_degree_prop", HelperService.CreateUri(TeacherConstants.DEGREE_PROP));
            queryString.SetUri("room_prop", HelperService.CreateUri(LessonConstants.ROOM_PROP));
            queryString.SetUri("room_name_prop", HelperService.CreateUri(RoomConstants.NAME_PROP));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("start_prop", HelperService.CreateUri(TimeslotConstants.START_TIME_PROP));
            queryString.SetUri("end_prop", HelperService.CreateUri(TimeslotConstants.END_TIME_PROP));
            queryString.SetUri("day_prop", HelperService.CreateUri(TimeslotConstants.DAY_PROP));

            var results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var lesson = new Lesson();
                    lesson.Id = result.Value("id").ToString().GetUriValue();
                    lesson.Type = result.Value("type").ToString();
                    lesson.Name = result.Value("name").ToString();
                    if (result.Value("numOfStudents") != null)
                        lesson.NumberOfStudents = result.Value("numOfStudents").AsValuedNode().AsInteger();
                    if (result.Value("group") != null) 
                        lesson.Group = result.Value("group").AsValuedNode().AsInteger();

                    var subject = new SchoolSubject();
                    subject.Id = result.Value("subjectId").ToString().GetUriValue();
                    subject.Name = result.Value("subjectName").ToString();
                    subject.Abbr = result.Value("subjectAbbr").ToString();
                    subject.NumberOfGroups = result.Value("subjectNumOfGroups").AsValuedNode().AsInteger();

                    var timeslot = new Timeslot();
                    timeslot.Id = result.Value("timeslotId").ToString().GetUriValue();
                    timeslot.StartTime = result.Value("start").AsValuedNode().AsString();
                    timeslot.EndTime = result.Value("end").AsValuedNode().AsString();
                    timeslot.Day = result.Value("day").ToString();

                    var teacher = new Teacher();
                    teacher.Id = result.Value("teacherId").ToString().GetUriValue();
                    teacher.Name = result.Value("teacherName").ToString();
                    teacher.Degree = result.Value("teacherDegree").ToString();

                    var room = new Room();
                    room.Id = result.Value("roomId").ToString().GetUriValue();
                    room.Name = result.Value("roomName").ToString();

                    lesson.SchoolSubject = subject;
                    lesson.Timeslot = timeslot;
                    lesson.Teacher = teacher;
                    lesson.Room = room;

                    lessons.Add(lesson);
                }
            }
            return lessons;
        }

        public async Task<List<Lesson>> GetUnassignedLessonsForStudyProgramme(string studyProgrammeId)
        {
            var lessons = new List<Lesson>();
            var queryString = new SparqlParameterizedString();
            queryString.CommandText = "" +
                "SELECT DISTINCT ?id ?name ?type ?group ?numOfStudents ?subjectId ?subjectName ?subjectAbbr ?subjectNumOfGroups ?lecturesNum ?practiceNum ?teacherId ?teacherName ?teacherDegree " +
                "FROM NAMED @graph_name " +
                "WHERE { " +
                "   GRAPH ?g { " +
                "       ?id a @class_name. " +
                "       ?id @name_prop ?name. " +
                "       ?id @type_prop ?type. " +
                "       OPTIONAL { " +
                "           ?id @num_studs_prop ?numOfStudents. " +
                "       } " +
                "       OPTIONAL { " +
                "           ?id @group_prop ?group. " +
                "       } " +
                "       ?id @subject_prop ?subjectId. " +
                "       ?subjectId @programme_prop @programme_id. " +
                "       ?subjectId @lectures_num_prop ?lecturesNum. " +
                "       ?subjectId @practice_num_prop ?practiceNum. " +
                "       ?subjectId @subject_name_prop ?subjectName. " +
                "       ?subjectId @subject_abbr_prop ?subjectAbbr. " +
                "       ?subjectId @subject_group_prop ?subjectNumOfGroups. " +
                "       ?id @teacher_prop ?teacherId. " +
                "       ?teacherId @teacher_name_prop ?teacherName. " +
                "       ?teacherId @teacher_degree_prop ?teacherDegree. " +
                "       FILTER NOT EXISTS { " +
                "           ?id @timeslot_prop ?timeslot. " +
                "       } " +
                "   } " +
                "} ";

            queryString.SetUri("graph_name", new Uri(DbConstants.GRAPH_NAME));
            queryString.SetUri("class_name", HelperService.CreateUri(LessonConstants.CLASS_NAME));
            queryString.SetUri("name_prop", HelperService.CreateUri(LessonConstants.NAME_PROP));
            queryString.SetUri("type_prop", HelperService.CreateUri(LessonConstants.TYPE_PROP));
            queryString.SetUri("num_studs_prop", HelperService.CreateUri(LessonConstants.NUM_OF_STUDENTS));
            //queryString.SetLiteral("type_value", lessonType);
            queryString.SetUri("group_prop", HelperService.CreateUri(LessonConstants.GROUP_PROP));
            //queryString.SetLiteral("group_value", group);
            queryString.SetUri("subject_prop", HelperService.CreateUri(LessonConstants.SUBJECT_PROP));
            queryString.SetUri("programme_prop", HelperService.CreateUri(SchoolSubjectConstants.STUDY_PROG_PROP));
            queryString.SetUri("programme_id", HelperService.CreateUri(studyProgrammeId));
            queryString.SetUri("lectures_num_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_LECTURES_PROP));
            queryString.SetUri("practice_num_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_PRACTICE_PROP));
            queryString.SetUri("subject_name_prop", HelperService.CreateUri(SchoolSubjectConstants.NAME_PROP));
            queryString.SetUri("subject_abbr_prop", HelperService.CreateUri(SchoolSubjectConstants.ABBR_PROP));
            queryString.SetUri("subject_group_prop", HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_GROUPS_PROP));
            queryString.SetUri("teacher_prop", HelperService.CreateUri(LessonConstants.TEACHER_PROP));
            queryString.SetUri("teacher_name_prop", HelperService.CreateUri(TeacherConstants.NAME_PROP));
            queryString.SetUri("teacher_degree_prop", HelperService.CreateUri(TeacherConstants.DEGREE_PROP));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));

            var results = await _repository.GetAsync(queryString.ToString());

            if (results is SparqlResultSet)
            {
                SparqlResultSet set = (SparqlResultSet)results;
                foreach (SparqlResult result in set)
                {
                    var lesson = new Lesson();
                    lesson.Id = result.Value("id").ToString().GetUriValue();
                    lesson.Name = result.Value("name").ToString();
                    lesson.Type = result.Value("type").ToString();
                    if (result.Value("numOfStudents") != null)
                        lesson.NumberOfStudents = result.Value("numOfStudents").AsValuedNode().AsInteger();
                    if (result.Value("group") != null)
                        lesson.Group = result.Value("group").AsValuedNode().AsInteger();

                    var subject = new SchoolSubject();
                    subject.Id = result.Value("subjectId").ToString().GetUriValue();
                    subject.Name = result.Value("subjectName").ToString();
                    subject.Abbr = result.Value("subjectAbbr").ToString();
                    subject.LecturesPerWeek = result.Value("lecturesNum").AsValuedNode().AsInteger();
                    subject.PracticePerWeek = result.Value("practiceNum").AsValuedNode().AsInteger();
                    subject.NumberOfGroups = result.Value("subjectNumOfGroups").AsValuedNode().AsInteger();

                    var teacher = new Teacher();
                    teacher.Id = result.Value("teacherId").ToString().GetUriValue();
                    teacher.Name = result.Value("teacherName").ToString();
                    teacher.Degree = result.Value("teacherDegree").ToString();
                    
                    lesson.SchoolSubject = subject;
                    lesson.Teacher = teacher;

                    lessons.Add(lesson);
                }
            }
            return lessons;
        }
    }
}
