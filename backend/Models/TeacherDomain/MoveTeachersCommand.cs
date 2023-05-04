namespace backend.Models.TeacherDomain
{
    public class MoveTeachersCommand
    {
        public List<string> TeacherIds;
        public string NewDepartmentId;
        public string PreviousDepartmentId;
    }
}
