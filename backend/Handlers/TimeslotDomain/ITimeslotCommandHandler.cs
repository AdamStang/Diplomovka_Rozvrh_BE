using backend.Models.TimeslotDomain;

namespace backend.Handlers.TimeslotDomain
{
    public interface ITimeslotCommandHandler
    {
        Task CreateTimeslots(List<Timeslot> timeslots);
        Task DeleteTimeslots();
    }
}