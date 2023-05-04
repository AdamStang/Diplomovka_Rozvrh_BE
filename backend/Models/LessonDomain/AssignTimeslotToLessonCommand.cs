namespace backend.Models.LessonDomain
{
    public class AssignTimeslotToLessonCommand
    {
        public AssignTimeslotToLessonCommand(string lessonId, string timeslotId, string roomId) 
        {
            LessonId = lessonId;
            TimeslotId = timeslotId;
            RoomId = roomId;
        }

        public string LessonId { get; set; }
        public string TimeslotId { get; set; }
        public string RoomId { get; set; }
        public bool Constraint { get; set; }
        public string TeacherId { get; set; }
    }
}
