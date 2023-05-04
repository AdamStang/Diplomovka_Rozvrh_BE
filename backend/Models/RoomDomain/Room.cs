using backend.Extensions;
using backend.Services;

namespace backend.Models.RoomDomain
{
    public class Room: StringModel
    {
        public Room(): base() { }

        public string Name { get; set; }
        public long Capacity { get; set; }
        public string Type { get; set; }

        public string CreateRoomId()
        {
            if (!string.IsNullOrEmpty(Id)) return Id;
            var hash = Hashing.ToHash(Name.ToSnakeCase());
            return $"{RoomConstants.CLASS_NAME}_{Name.ToSnakeCase()}_{hash}";
        }
    }
}
