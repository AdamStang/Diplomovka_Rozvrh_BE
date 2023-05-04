using backend.Models.TeacherDomain;

namespace backend.Handlers.TeacherDomain
{
    public interface ITeacherCommandHandler
    {
        Task CreateTeacher(Teacher teacher);
        Task DeleteTeacher(string teacherId);
        Task UpdateTeacher(Teacher teacher);
        Task MoveTeachersToNewDepartment(MoveTeachersCommand teachersCommand);
        Task UpdateTimeConstraintsForTeacher(UpdateTimeConstraintsCommand constraintsCommand);
    }
}