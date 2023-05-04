using backend.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF;
using backend.Services;
using backend.Models;
using VDS.RDF.Nodes;
using backend.Extensions;
using backend.Models.TeacherDomain;
using backend.Handlers.TeacherDomain;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherQueryHandler _queryHandler;
        private readonly ITeacherCommandHandler _commandHandler;

        public TeachersController(ITeacherQueryHandler queryHandler, ITeacherCommandHandler commandHandler)
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpGet("getAllTeachers")]
        public async Task<List<Teacher>> GetAllTeachers()
        {
            return await _queryHandler.GetAllTeachers();
        }

        [HttpGet("getTeachersCount")]
        public async Task<long> GetTeachersCount()
        {
            return await _queryHandler.GetTeachersCount();
        }

        [HttpGet("getTeachersByDepartment")]
        public async Task<List<Teacher>> GetTeachersByDepartment(string departmentId)
        {
            return await _queryHandler.GetTeachersByDepartment(departmentId);
        }

        [HttpPost("createTeacher")]
        public async Task CreateTeacher(Teacher teacher)
        {
            await _commandHandler.CreateTeacher(teacher);
        }

        [HttpDelete("deleteTeacher")]
        public async Task DeleteTeacher(string teacherId)
        {
            await _commandHandler.DeleteTeacher(teacherId);
        }

        [HttpGet("checkTeacherTimeCollision")]
        public async Task<bool> CheckTeacherTimeCollision(string teacherid, string timeslotId)
        {
            return await _queryHandler.CheckTeacherTimeCollision(teacherid, timeslotId);
        }

        [HttpPatch("updateTeacher")]
        public async Task UpdateTeacher(Teacher teacher)
        {
            await _commandHandler.UpdateTeacher(teacher);
        }

        [HttpPost("moveTeachersToNewDepartment")]
        public async Task MoveTeachersToNewDepartment(MoveTeachersCommand teachersCommand)
        {
            await _commandHandler.MoveTeachersToNewDepartment(teachersCommand);
        }

        [HttpPost("updateTimeConstraintsForTeacher")]
        public async Task UpdateTimeConstraintsForTeacher(UpdateTimeConstraintsCommand constraintsCommand)
        {
            await _commandHandler.UpdateTimeConstraintsForTeacher(constraintsCommand);
        }

        [HttpGet("getTimeslotConstraints")]
        public async Task<List<string>> GetTimeslotConstraints(string teacherId)
        {
            return await _queryHandler.GetTimeslotConstraints(teacherId);
        }

        [HttpGet("checkTeacherTimeslotConstraint")]
        public async Task<bool> CheckTeacherTimeslotCnstraint(string teacherId, string timeslotId)
        {
            return await _queryHandler.CheckTeacherTimeslotConstraint(teacherId, timeslotId);
        }
    }
}
