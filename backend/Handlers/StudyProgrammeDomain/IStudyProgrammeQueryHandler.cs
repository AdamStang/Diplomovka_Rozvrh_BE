using backend.Models.StudyProgrammeDomain;

namespace backend.Handlers.StudyProgrammeDomain
{
    public interface IStudyProgrammeQueryHandler
    {
        Task<List<StudyProgramme>> GetAllStudyProgrammes();
    }
}