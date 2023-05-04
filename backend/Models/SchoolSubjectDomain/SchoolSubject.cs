using backend.Extensions;
using backend.Models.LessonDomain;
using backend.Models.StudyProgrammeDomain;
using backend.Models.TeacherDomain;
using backend.Services;

namespace backend.Models.SchoolSubjectDomain
{
    public class SchoolSubject: StringModel
    {
        public SchoolSubject(): base() { }

        public string Name { get; set; }
        public string Abbr { get; set; }
        public long NumberOfStudents { get; set; }
        public long LecturesPerWeek { get; set; }
        public long PracticePerWeek { get; set; }
        public long NumberOfGroups { get; set; }
        public List<Lesson>? Lessons { get; set; }
        public Teacher? Teacher { get; set; }
        public StudyProgramme? StudyProgramme { get; set; }

        public string CreateSchoolSubjectId()
        {
            if (!string.IsNullOrEmpty(Id)) return Id;
            var hash = Hashing.ToHash(Name.ToSnakeCase());
            return $"{SchoolSubjectConstants.CLASS_NAME}_{Name.ToSnakeCase()}_{hash}";
        }
    }
}
