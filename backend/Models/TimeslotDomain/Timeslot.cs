using backend.Handlers.TeacherDomain;
using backend.Models.DepartmentDomain;
using backend.Models.SchoolSubjectDomain;
using backend.Services;
using Newtonsoft.Json;

namespace backend.Models.TimeslotDomain
{
    public class Timeslot: StringModel
    {
        public Timeslot(): base() { }

        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string CreateTimeslotId()
        {
            if (!string.IsNullOrEmpty(Id)) return Id;
            var hash = Hashing.ToHash($"{StartTime}_{Day}");
            return $"{TimeslotConstants.CLASS_NAME}_{StartTime}_{hash}";
        }
    }
}
