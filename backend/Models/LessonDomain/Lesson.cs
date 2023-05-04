using backend.Extensions;
using backend.Models.RoomDomain;
using backend.Models.SchoolSubjectDomain;
using backend.Models.TeacherDomain;
using backend.Models.TimeslotDomain;
using backend.Services;

namespace backend.Models.LessonDomain
{
    public class Lesson: StringModel
    {
        public string Name { get; set; }
        public long NumberOfStudents { get; set; }
        public long? Group { get; set; }
        public Teacher Teacher { get; set; }
        public Room? Room { get; set; }
        public Timeslot? Timeslot { get; set; }
        public SchoolSubject SchoolSubject { get; set; }
        public string Type { get; set; }

        public string CreateId()
        {
            if (!string.IsNullOrEmpty(Id)) return Id;
            var hash = Hashing.ToHash($"{Name.RemoveDiacritics().ToSnakeCase()}_{Type.RemoveDiacritics()}_{SchoolSubject.Name.RemoveDiacritics().ToSnakeCase()}");
            return $"{LessonConstants.CLASS_NAME}_{Name.RemoveDiacritics().ToSnakeCase()}_{Type.RemoveDiacritics()}_{SchoolSubject.Name.RemoveDiacritics().ToSnakeCase()}_{hash}";
        }
    }
}
