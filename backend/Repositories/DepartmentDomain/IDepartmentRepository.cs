using VDS.RDF;

namespace backend.Repositories.DepartmentDomain
{
    public interface IDepartmentRepository
    {
        Task CreateAsync(List<Triple> triples);
        Task DeleteAsync(string query);
        Task<object> GetAsync(string query);
        Graph GetGraph();
        Task UpdateAsync(List<Triple> triples);
    }
}