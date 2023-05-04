using backend.Extensions;
using backend.Handlers.SchoolSubjectDomain;
using backend.Models.DepartmentDomain;
using backend.Models.LessonDomain;
using backend.Models.SchoolSubjectDomain;
using backend.Models.TimeslotDomain;
using backend.Services;

namespace backend.Models.TeacherDomain
{
    public class Teacher: StringModel
    {
        public Teacher(): base() { }

        public string Name { get; set; }
        public string Degree { get; set; }
        public Department? Department { get; set; }
        public List<SchoolSubject>? Subjects { get; set; }
        public List<Lesson>? Lessons { get; set; }
        public List<string>? TimeConstraints { get; set; }

        public string CreateTeacherId()
        {
            if (!string.IsNullOrEmpty(Id)) return Id;
            var hash = Hashing.ToHash($"{Name.ToSnakeCase()}_{Degree.ToSnakeCase()}");
            return $"{TeacherConstants.CLASS_NAME}_{Name.ToSnakeCase()}_{Degree.ToSnakeCase()}_{hash}";
        }
    }
}
