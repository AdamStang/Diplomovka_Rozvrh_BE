using backend.Models.LessonDomain;

namespace backend.Handlers.LessonDomain
{
    public interface ILessonCommandHandler
    {
        Task AssignTimeslotAndRoomToLesson(AssignTimeslotToLessonCommand lessonCommand);
        Task DeleteTimeslotAndRoom(string lessonId);
        Task EditLesson(EditLessonCommand lessonCommand);
    }
}