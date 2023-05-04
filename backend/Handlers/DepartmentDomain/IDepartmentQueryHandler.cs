using backend.Models.DepartmentDomain;

namespace backend.Handlers.DepartmentDomain
{
    public interface IDepartmentQueryHandler
    {
        Task<List<Department>> GetAllDepartments();
        Task<long> GetDepartmentsCount();
    }
}