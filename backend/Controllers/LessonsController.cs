using backend.Handlers.LessonDomain;
using backend.Models.LessonDomain;
using backend.Models.SchoolSubjectDomain;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonQueryHandler _queryHandler;
        private readonly ILessonCommandHandler _commandHandler;

        public LessonsController(ILessonQueryHandler queryHandler, ILessonCommandHandler commandHandler) 
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpGet("getLessonsInRoom")]
        public async Task<List<Lesson>> GetLessonsInRoom(string roomId)
        {
            return await _queryHandler.GetLessonsInRoom(roomId);
        }

        [HttpGet("getLessonsForTeacher")]
        public async Task<List<Lesson>> GetLessonsForTeacher(string teacherId)
        {
            return await _queryHandler.GetLessonsForTeacher(teacherId);
        }

        [HttpGet("getAssignedLessonsForStudyProgramme")]
        public async Task<List<Lesson>> GetAssignedLessonsForStudyProgramme(string studyProgrammeId)
        {
            return await _queryHandler.GetAssignedLessonsForStudyProgramme(studyProgrammeId);
        }

        [HttpGet("getUnassignedLessonsForStudyProgramme")]
        public async Task<List<Lesson>> GetUnassignedLessonsForStudyProgramme(string studyProgrammeId)
        {
            return await _queryHandler.GetUnassignedLessonsForStudyProgramme(studyProgrammeId);
        }

        [HttpPost("assignTimeslotAndRoomToLesson")]
        public async Task AssignTimeslotAndRoomToLesson(AssignTimeslotToLessonCommand lessonCommand)
        {
            await _commandHandler.AssignTimeslotAndRoomToLesson(lessonCommand);
        }

        [HttpDelete("deleteTimeslotAndRoom")]
        public async Task DeleteTimeslotAndRoom(string lessonId)
        {
            await _commandHandler.DeleteTimeslotAndRoom(lessonId);
        }

        [HttpPost("editLesson")]
        public async Task EditLesson(EditLessonCommand lessonCommand)
        {
            await _commandHandler.EditLesson(lessonCommand);
        }
    }
}
