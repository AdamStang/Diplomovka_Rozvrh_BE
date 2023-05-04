using VDS.RDF;

namespace backend.Repositories.RoomDomain
{
    public interface IRoomRepository
    {
        public Task<object> GetAsync(string query);
        public Task CreateAsync(List<Triple> triples);
        public Task UpdateAsync(List<Triple> triples);
        public Task DeleteAsync(string query);
        public Graph GetGraph();
    }
}
