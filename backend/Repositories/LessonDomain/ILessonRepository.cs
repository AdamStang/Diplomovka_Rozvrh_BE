using VDS.RDF;

namespace backend.Repositories.LessonDomain
{
    public interface ILessonRepository
    {
        Task CreateAsync(List<Triple> triples);
        Task DeleteAsync(string query);
        Task<object> GetAsync(string query);
        Graph GetGraph();
        Task UpdateAsync(List<Triple> newTriples, List<Triple> previousTriples);
    }
}