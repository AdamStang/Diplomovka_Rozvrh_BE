using backend.Models.LessonDomain;
using backend.Models.SchoolSubjectDomain;

namespace backend.Handlers.LessonDomain
{
    public interface ILessonQueryHandler
    {
        Task<List<Lesson>> GetLessonsForTeacher(string teacherId);
        Task<List<Lesson>> GetLessonsInRoom(string roomId);
        Task<List<Lesson>> GetAssignedLessonsForStudyProgramme(string studyProgrammeId);
        Task<List<Lesson>> GetUnassignedLessonsForStudyProgramme(string studyProgrammeId);
    }
}