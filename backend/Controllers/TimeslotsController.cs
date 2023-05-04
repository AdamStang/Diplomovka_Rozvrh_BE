using Microsoft.AspNetCore.Mvc;
using backend.Models.TimeslotDomain;
using backend.Handlers.TimeslotDomain;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeslotsController : ControllerBase
    {
        private readonly ITimeslotQueryHandler _queryHandler;
        private readonly ITimeslotCommandHandler _commandHandler;

        public TimeslotsController(ITimeslotQueryHandler queryHandler, ITimeslotCommandHandler commandHandler)
        {
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        [HttpGet("getAllTimeslots")]
        public async Task<List<Timeslot>> GetAllTimeslots()
        {
            return await _queryHandler.GetAllTimeslots();
        }

        [HttpPost("createTimeslots")]
        public async Task CreateTimeslots(List<Timeslot> timeslots)
        {
            await _commandHandler.CreateTimeslots(timeslots);
        }
    }
}
