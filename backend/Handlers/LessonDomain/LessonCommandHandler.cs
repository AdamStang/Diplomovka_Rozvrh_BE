using backend.Models.LessonDomain;
using backend.Models.TeacherDomain;
using backend.Repositories.LessonDomain;
using backend.Services;
using VDS.RDF;
using VDS.RDF.Query;

namespace backend.Handlers.LessonDomain
{
    public class LessonCommandHandler : ILessonCommandHandler
    {
        private readonly ILessonRepository _repository;

        public LessonCommandHandler(ILessonRepository repository)
        {
            _repository = repository;
        }

        public async Task AssignTimeslotAndRoomToLesson(AssignTimeslotToLessonCommand lessonCommand)
        {
            await DeleteTimeslotAndRoom(lessonCommand.LessonId);

            var teacherTriples = new List<Triple>();

            if (lessonCommand.Constraint)
            {
                INode teacherSubject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.TeacherId));
                INode teacherPredicate = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(TeacherConstants.CONSTRAINTS_PROP));
                INode teacherObject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.TimeslotId));
                teacherTriples.Add(new Triple(teacherSubject, teacherPredicate, teacherObject));
            }

            var lessonTriples = new List<Triple>();

            INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.LessonId));
            INode predicateIsInTimeslot = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            INode objTimeslot = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.TimeslotId));
            lessonTriples.Add(new Triple(subject, predicateIsInTimeslot, objTimeslot));

            INode predicateIsInsideRoom = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.ROOM_PROP));
            INode objRoom = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.RoomId));
            lessonTriples.Add(new Triple(subject, predicateIsInsideRoom, objRoom));

            await _repository.UpdateAsync(lessonTriples, teacherTriples);
        }

        public async Task EditLesson(EditLessonCommand lessonCommand)
        {
            var newTripples = new List<Triple>();
            var previousTripples = new List<Triple>();

            INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.Lesson.Id));

            if (lessonCommand.NewTimeslot != null)
            {
                INode predicateIsInTimeslot = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
                INode objTimeslot = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.NewTimeslot.Id));
                newTripples.Add(new Triple(subject, predicateIsInTimeslot, objTimeslot));
            }

            if (lessonCommand.NewRoom != null)
            {
                INode predicateIsInsideRoom = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.ROOM_PROP));
                INode objRoom = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.NewRoom.Id));
                newTripples.Add(new Triple(subject, predicateIsInsideRoom, objRoom));
            }

            if (lessonCommand.NewTeacher != null)
            {
                INode predicatehasTeacher = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TEACHER_PROP));
                INode objTeacher = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.NewTeacher.Id));
                newTripples.Add(new Triple(subject, predicatehasTeacher, objTeacher));
            }

            if (newTripples.Count <= 0) return;

            if (lessonCommand.Lesson.Timeslot != null)
            {
                INode predicateIsInTimeslot = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
                INode objTimeslot = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.Lesson.Timeslot.Id));
                previousTripples.Add(new Triple(subject, predicateIsInTimeslot, objTimeslot));
            }

            if (lessonCommand.Lesson.Room != null)
            {
                INode predicateIsInsideRoom = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.ROOM_PROP));
                INode objRoom = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.Lesson.Room.Id));
                previousTripples.Add(new Triple(subject, predicateIsInsideRoom, objRoom));
            }

            if (lessonCommand.Lesson.Teacher != null)
            {
                INode predicatehasTeacher = _repository.GetGraph().GetUriNode(HelperService.CreateUri(LessonConstants.TEACHER_PROP));
                INode objTeacher = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(lessonCommand.Lesson.Teacher.Id));
                previousTripples.Add(new Triple(subject, predicatehasTeacher, objTeacher));
            }

            await _repository.UpdateAsync(newTripples, previousTripples);
        }

        public async Task DeleteTimeslotAndRoom(string  lessonId)
        {
            var queryString = new SparqlParameterizedString();

            queryString.CommandText = "DELETE { GRAPH ?g { @lesson_id @timeslot_prop ?timeslot. @lesson_id @room_prop ?room. } } WHERE { GRAPH ?g { @lesson_id @timeslot_prop ?timeslot. @lesson_id @room_prop ?room. } }";
            queryString.SetUri("lesson_id", HelperService.CreateUri(lessonId));
            queryString.SetUri("timeslot_prop", HelperService.CreateUri(LessonConstants.TIMESLOT_PROP));
            queryString.SetUri("room_prop", HelperService.CreateUri(LessonConstants.ROOM_PROP));

            await _repository.DeleteAsync(queryString.ToString());
        }
    }
}
