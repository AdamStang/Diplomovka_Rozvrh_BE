using backend.Handlers.StudyProgrammeDomain;
using backend.Models.StudyProgrammeDomain;
using backend.Models.TeacherDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyProgrammesController : ControllerBase
    {
        private readonly IStudyProgrammeQueryHandler _queryHandler;
        private readonly IStudyProgrammeCommandHandler _commandHandler;

        public StudyProgrammesController(IStudyProgrammeQueryHandler queryHandler, IStudyProgrammeCommandHandler commandHandler) 
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpGet("getAllStudyProgrammes")]
        public async Task<List<StudyProgramme>> GetAllStudyProgrammes()
        {
            return await _queryHandler.GetAllStudyProgrammes();
        }

        [HttpPost("createStudyProgramme")]
        public async Task CreateStudyProgramme(StudyProgramme studyProgramme)
        {
            await _commandHandler.CreateStudyProgramme(studyProgramme);
        }

        [HttpDelete("deleteStudyProgramme")]
        public async Task DeleteStudyProgramme(string studyProgrammeId)
        {
            await _commandHandler.DeleteStudyProgramme(studyProgrammeId);
        }
    }
}
