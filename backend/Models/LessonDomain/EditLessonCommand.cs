using backend.Models.RoomDomain;
using backend.Models.TeacherDomain;
using backend.Models.TimeslotDomain;

namespace backend.Models.LessonDomain
{
    public class EditLessonCommand
    {
        public Lesson Lesson { get; set; }
        public Room NewRoom { get; set; }
        public Timeslot NewTimeslot { get; set; }
        public Teacher NewTeacher { get; set; }

    }
}
