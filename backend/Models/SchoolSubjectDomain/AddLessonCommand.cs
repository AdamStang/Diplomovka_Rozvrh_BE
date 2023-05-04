using backend.Models.LessonDomain;

namespace backend.Models.SchoolSubjectDomain
{
    public class AddLessonCommand
    {
        public string SchoolSubjectId;
        public Lesson NewLesson;
    }
}
