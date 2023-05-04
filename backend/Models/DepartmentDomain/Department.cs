using backend.Extensions;
using backend.Services;

namespace backend.Models.DepartmentDomain
{
    public class Department: StringModel
    {
        public Department(): base() { }

        public string Name { get; set; }
        public string Abbr { get; set; }

        public string CreateDepartmentId()
        {
            if (!string.IsNullOrEmpty(Id)) return Id;
            var hash = Hashing.ToHash(Name.ToSnakeCase());
            return $"{DepartmentConstants.CLASS_NAME}_{Abbr.ToSnakeCase()}_{hash}";
        }
    }
}
