using backend.Models.SchoolSubjectDomain;

namespace backend.Handlers.SchoolSubjectDomain
{
    public interface ISchoolSubjectCommandHandler
    {
        Task CreateSchoolSubject(SchoolSubject schoolSubject);
        Task AddLesson(AddLessonCommand addLessonCommand);
        Task DeleteSchoolSubject(string schoolSubjectId);
    }
}