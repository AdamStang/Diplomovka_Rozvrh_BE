using backend.Handlers.SchoolSubjectDomain;
using backend.Models.LessonDomain;
using backend.Models.SchoolSubjectDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolSubjectsController : ControllerBase
    {
        private readonly ISchoolSubjectQueryHandler _queryHandler;
        private readonly ISchoolSubjectCommandHandler _commandHandler;

        public SchoolSubjectsController(ISchoolSubjectQueryHandler queryHandler, ISchoolSubjectCommandHandler commandHandler)
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpGet("getAllSchoolSubjects")]
        public async Task<List<SchoolSubject>> GetAllSchoolSubjects()
        {
            return await _queryHandler.GetAllSchoolSubjects();
        }

        [HttpGet("getSchoolSubjectsCount")]
        public async Task<long> GetSchoolSubjectsCount()
        {
            return await _queryHandler.GetSchoolSubjectsCount();
        }

        [HttpPost("createSchoolSubject")]
        public async Task CreateSchoolSubject(SchoolSubject schoolSubject)
        {
            await _commandHandler.CreateSchoolSubject(schoolSubject);
        }

        [HttpDelete("deleteSchoolSubject")]
        public async Task DeleteSchoolSubject(string schoolSubjectId)
        {
            await _commandHandler.DeleteSchoolSubject(schoolSubjectId);
        }

        [HttpGet("getSubjectsLessons")]
        public async Task<List<Lesson>> GetSubjectsLessons(string subjectId)
        {
            return await _queryHandler.GetSubjectsLessons(subjectId);
        }

        [HttpPost("addLesson")]
        public async Task AddLesson(AddLessonCommand addLessonCommand)
        {
            await _commandHandler.AddLesson(addLessonCommand);
        }
    }
}
