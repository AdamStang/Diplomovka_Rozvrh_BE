using backend.Models.DepartmentDomain;

namespace backend.Handlers.DepartmentDomain
{
    public interface IDepartmentCommandHandler
    {
        Task CreateDepartment(Department department);
        Task DeleteDepartment(string departmentId);
    }
}