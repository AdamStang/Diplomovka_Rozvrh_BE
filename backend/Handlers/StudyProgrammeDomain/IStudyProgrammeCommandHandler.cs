using backend.Models.StudyProgrammeDomain;

namespace backend.Handlers.StudyProgrammeDomain
{
    public interface IStudyProgrammeCommandHandler
    {
        Task CreateStudyProgramme(StudyProgramme studyProgramme);
        Task DeleteStudyProgramme(string studyProgrammeId);
    }
}