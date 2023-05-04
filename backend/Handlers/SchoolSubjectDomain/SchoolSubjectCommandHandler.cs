using backend.Repositories.SchoolSubjectDomain;
using backend.Services;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF;
using backend.Models.SchoolSubjectDomain;
using backend.Models.LessonDomain;

namespace backend.Handlers.SchoolSubjectDomain
{
    public class SchoolSubjectCommandHandler : ISchoolSubjectCommandHandler
    {
        private readonly ISchoolSubjectRepository _repository;

        public SchoolSubjectCommandHandler(ISchoolSubjectRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateSchoolSubject(SchoolSubject schoolSubject)
        {
            var id = schoolSubject.CreateSchoolSubjectId();
            var schoolSubjectTriples = new List<Triple>();

            INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(id));
            INode predicate = _repository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
            INode obj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.CLASS_NAME));
            schoolSubjectTriples.Add(new Triple(subject, predicate, obj));

            INode predicateHasName = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.NAME_PROP));
            INode objName = _repository.GetGraph().CreateLiteralNode(schoolSubject.Name);
            schoolSubjectTriples.Add(new Triple(subject, predicateHasName, objName));

            INode predicatehasAbbreviation = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.ABBR_PROP));
            INode objAbbreviation = _repository.GetGraph().CreateLiteralNode(schoolSubject.Abbr);
            schoolSubjectTriples.Add(new Triple(subject, predicatehasAbbreviation, objAbbreviation));

            INode predicatehasHasNumOfStudents = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_STUDENTS));
            INode objNumOfStudents = _repository.GetGraph().CreateLiteralNode(schoolSubject.NumberOfStudents.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
            schoolSubjectTriples.Add(new Triple(subject, predicatehasHasNumOfStudents, objNumOfStudents));

            INode predicatehasHasNumOfPractices = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_PRACTICE_PROP));
            INode objNumOfPractices = _repository.GetGraph().CreateLiteralNode(schoolSubject.PracticePerWeek.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
            schoolSubjectTriples.Add(new Triple(subject, predicatehasHasNumOfPractices, objNumOfPractices));

            INode predicatehasHasNumOfLectures = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_LECTURES_PROP));
            INode objNumOfLectures = _repository.GetGraph().CreateLiteralNode(schoolSubject.LecturesPerWeek.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
            schoolSubjectTriples.Add(new Triple(subject, predicatehasHasNumOfLectures, objNumOfLectures));

            INode predicatehasHasNumOfGroups = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.NUM_OF_GROUPS_PROP));
            INode objNumOfGroups = _repository.GetGraph().CreateLiteralNode(schoolSubject.NumberOfGroups.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
            schoolSubjectTriples.Add(new Triple(subject, predicatehasHasNumOfGroups, objNumOfGroups));

            INode studyProgrammePredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.STUDY_PROG_PROP));
            INode objStudyProgramme = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(schoolSubject.StudyProgramme.Id));
            schoolSubjectTriples.Add(new Triple(subject, studyProgrammePredicate, objStudyProgramme));

            if (schoolSubject.Teacher != null)
            {
                INode TeacherPredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.TEACHER_PROP));
                INode objTeacher = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(schoolSubject.Teacher.Id));
                schoolSubjectTriples.Add(new Triple(subject, TeacherPredicate, objTeacher));
            }

            if (schoolSubject?.Lessons?.Count > 0)
            {
                foreach (var lesson in schoolSubject.Lessons)
                {
                    var lessonId = lesson.CreateId();
                    INode lessonSubject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonId));
                    INode lessonPredicate = _repository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
                    INode lessonObj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.CLASS_NAME));
                    schoolSubjectTriples.Add(new Triple(lessonSubject, lessonPredicate, lessonObj));

                    INode lessonNamePredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.NAME_PROP));
                    INode lessonsNameObj = _repository.GetGraph().CreateLiteralNode(lesson.Name);
                    schoolSubjectTriples.Add(new Triple(lessonSubject, lessonNamePredicate, lessonsNameObj));

                    INode lessonTypePredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TYPE_PROP));
                    INode lessonTypeObj = _repository.GetGraph().CreateLiteralNode(lesson.Type);
                    schoolSubjectTriples.Add(new Triple(lessonSubject, lessonTypePredicate, lessonTypeObj));

                    INode lessonNumOfStudentsPredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.NUM_OF_STUDENTS));
                    INode lessonNumOfStudentsObj = _repository.GetGraph().CreateLiteralNode(lesson.NumberOfStudents.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
                    schoolSubjectTriples.Add(new Triple(lessonSubject, lessonNumOfStudentsPredicate, lessonNumOfStudentsObj));

                    if (lesson.Group != null)
                    {
                        INode lessonGroupPredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.GROUP_PROP));
                        INode lessoGroupObj = _repository.GetGraph().CreateLiteralNode(lesson.Group.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
                        schoolSubjectTriples.Add(new Triple(lessonSubject, lessonGroupPredicate, lessoGroupObj));
                    }

                    INode lessonTeacherPredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TEACHER_PROP));
                    INode lessonTeacherObj = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lesson.Teacher.Id));
                    schoolSubjectTriples.Add(new Triple(lessonSubject, lessonTeacherPredicate, lessonTeacherObj));

                    INode lessonSubjectPredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.SUBJECT_PROP));
                    INode lessonSubjectObj = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(id));
                    schoolSubjectTriples.Add(new Triple(lessonSubject, lessonSubjectPredicate, lessonSubjectObj));

                    INode predicateHasLesson = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.LESSONS_PROP));
                    schoolSubjectTriples.Add(new Triple(subject, predicateHasLesson, lessonSubject));
                }
            }

            await _repository.CreateAsync(schoolSubjectTriples);
        }

        public async Task AddLesson(AddLessonCommand addLessonCommand)
        {
            var lessonId = addLessonCommand.NewLesson.CreateId();
            var schoolSubjectTriples = new List<Triple>();

            INode lessonSubject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonId));
            INode lessonPredicate = _repository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
            INode lessonObj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.CLASS_NAME));
            schoolSubjectTriples.Add(new Triple(lessonSubject, lessonPredicate, lessonObj));

            INode lessonNamePredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.NAME_PROP));
            INode lessonsNameObj = _repository.GetGraph().CreateLiteralNode(addLessonCommand.NewLesson.Name);
            schoolSubjectTriples.Add(new Triple(lessonSubject, lessonNamePredicate, lessonsNameObj));

            INode lessonTypePredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TYPE_PROP));
            INode lessonTypeObj = _repository.GetGraph().CreateLiteralNode(addLessonCommand.NewLesson.Type);
            schoolSubjectTriples.Add(new Triple(lessonSubject, lessonTypePredicate, lessonTypeObj));

            INode lessonTeacherPredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TEACHER_PROP));
            INode lessonTeacherObj = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(addLessonCommand.NewLesson.Teacher.Id));
            schoolSubjectTriples.Add(new Triple(lessonSubject, lessonTeacherPredicate, lessonTeacherObj));

            INode lessonSubjectPredicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.SUBJECT_PROP));
            INode lessonSubjectObj = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(addLessonCommand.NewLesson.SchoolSubject.Id));
            schoolSubjectTriples.Add(new Triple(lessonSubject, lessonSubjectPredicate, lessonSubjectObj));


            INode subject = _repository.GetGraph().GetUriNode(HelperService.CreateUri(addLessonCommand.SchoolSubjectId));
            INode predicate = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.LESSONS_PROP));
            INode obj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.CLASS_NAME));
            schoolSubjectTriples.Add(new Triple(subject, predicate, obj));

            INode predicateHasLesson = _repository.GetGraph().GetUriNode(HelperService.CreateUri(SchoolSubjectConstants.LESSONS_PROP));
            schoolSubjectTriples.Add(new Triple(subject, predicateHasLesson, lessonSubject));

            await _repository.CreateAsync(schoolSubjectTriples);
        }

        public async Task DeleteSchoolSubject(string schoolSubjectId)
        {
            var queryString = new SparqlParameterizedString();

            queryString.CommandText = "DELETE { GRAPH ?g { @id ?p1 ?o. ?s ?p2 @id. ?s2 @subject_prop @id. ?s2 ?p3 ?o3. } } WHERE { Graph ?g { @id ?p1 ?o. OPTIONAL { ?s ?p2 @id. } OPTIONAL { ?s2 @subject_prop @id. ?s2 ?p3 ?o3. } } }";
            queryString.SetUri("id", HelperService.CreateUri(schoolSubjectId));
            queryString.SetUri("subject_prop", HelperService.CreateUri(LessonConstants.SUBJECT_PROP));

            await _repository.DeleteAsync(queryString.ToString());
        }
    }
}
