namespace backend.Models.TeacherDomain
{
    public class UpdateTimeConstraintsCommand
    {
        public string TeacherId { get; set; }
        public List<string> TimeslotsIds { get; set; }
    }
}
