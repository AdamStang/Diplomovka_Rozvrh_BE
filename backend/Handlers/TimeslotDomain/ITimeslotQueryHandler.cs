using backend.Models.TimeslotDomain;

namespace backend.Handlers.TimeslotDomain
{
    public interface ITimeslotQueryHandler
    {
        Task<List<Timeslot>> GetAllTimeslots();
    }
}