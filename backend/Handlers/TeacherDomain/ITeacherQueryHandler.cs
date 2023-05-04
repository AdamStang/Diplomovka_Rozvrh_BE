using backend.Models.TeacherDomain;

namespace backend.Handlers.TeacherDomain
{
    public interface ITeacherQueryHandler
    {
        Task<List<Teacher>> GetAllTeachers();
        Task<List<Teacher>> GetTeachersByDepartment(string departmentId);
        Task<long> GetTeachersCount();
        Task<bool> CheckTeacherTimeCollision(string teacherId, string timeslotId);
        Task<List<string>> GetTimeslotConstraints(string teacherId);
        Task<bool> CheckTeacherTimeslotConstraint(string teacherId, string timeslotId);
    }
}