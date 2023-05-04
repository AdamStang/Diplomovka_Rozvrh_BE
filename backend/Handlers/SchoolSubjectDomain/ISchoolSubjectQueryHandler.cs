using backend.Models.LessonDomain;
using backend.Models.SchoolSubjectDomain;

namespace backend.Handlers.SchoolSubjectDomain
{
    public interface ISchoolSubjectQueryHandler
    {
        Task<List<SchoolSubject>> GetAllSchoolSubjects();
        Task<long> GetSchoolSubjectsCount();
        Task<List<Lesson>> GetSubjectsLessons(string subjectId);
    }
}