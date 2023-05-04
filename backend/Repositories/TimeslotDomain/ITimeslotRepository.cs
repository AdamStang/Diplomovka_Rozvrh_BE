using VDS.RDF;

namespace backend.Repositories.TimeslotDomain
{
    public interface ITimeslotRepository
    {
        Task CreateAsync(List<Triple> triples);
        Task DeleteAsync(string query);
        Task<object> GetAsync(string query);
        Graph GetGraph();
        Task UpdateAsync(List<Triple> triples);
    }
}