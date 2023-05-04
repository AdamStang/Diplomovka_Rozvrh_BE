using backend.Extensions;
using backend.Services;

namespace backend.Models.StudyProgrammeDomain
{
    public class StudyProgramme: StringModel
    {
        public StudyProgramme(): base() { }

        public string Name { get; set; }
        public string Abbr { get; set; }
        public long Grade { get; set; }
        public long NumberOfStudents { get; set; }
        public long NumberOfGroups { get; set; }
        //public List<ParallelBlockDTO>? SubParallelBlock { get; set; }

        public string CreateStudyProgrammeId()
        {
            if (!string.IsNullOrEmpty(Id)) return Id;
            var hash = Hashing.ToHash($"{Name.ToSnakeCase()}_{Grade}");
            return $"{StudyProgrammeConstants.CLASS_NAME}_{Name.ToSnakeCase()}_{hash}";
        }
    }
}
