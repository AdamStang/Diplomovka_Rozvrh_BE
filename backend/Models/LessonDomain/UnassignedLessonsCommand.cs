using backend.Models.StudyProgrammeDomain;

namespace backend.Models.LessonDomain
{
    public class UnassignedLessonsCommand
    {
        public string StudyProgrammeId { get; set; }
        public string LessonType { get; set; }
        public long Group { get; set; }
    }
}
