using backend.Handlers.DepartmentDomain;
using backend.Models.DepartmentDomain;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentQueryHandler _queryHandler;
        private readonly IDepartmentCommandHandler _commandHandler;

        public DepartmentsController(IDepartmentQueryHandler queryHandler, IDepartmentCommandHandler commandHandler) 
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpGet("getAllDepartments")]
        public async Task<List<Department>> GetAllDepartments()
        {
            return await _queryHandler.GetAllDepartments();
        }

        [HttpGet("getDepartmentsCount")]
        public async Task<long> GetDepartmentsCount()
        {
            return await _queryHandler.GetDepartmentsCount();
        }

        [HttpPost("createDepartment")]
        public async Task CreateDepartment(Department department)
        {
            await _commandHandler.CreateDepartment(department);
        }

        [HttpDelete("deleteDepartment")]
        public async Task DeleteDepartment(string departmentId)
        {
            await _commandHandler.DeleteDepartment(departmentId);
        }
    }
}
