using backend.Database;
using VDS.RDF;

namespace backend.Repositories.StudyProgrammeDomain
{
    public class StudyProgrammeRepository : IStudyProgrammeRepository
    {
        private readonly IDbContext _context;

        public StudyProgrammeRepository(IDbContext context)
        {
            _context = context;
            _context.MyStardog.LoadGraph(_context.MyGraph, new Uri(DbConstants.GRAPH_NAME));
        }

        public async Task<object> GetAsync(string query)
        {
            return _context.MyStardog.Query(query);
        }

        public async Task CreateAsync(List<Triple> triples)
        {
            if (_context.MyStardog.UpdateSupported)
            {
                _context.MyStardog.UpdateGraph(new Uri(DbConstants.GRAPH_NAME), triples, null);
            }
            else
            {
                throw new Exception("Store does not support creating new tripples.");
            }
        }

        public async Task UpdateAsync(List<Triple> triples)
        {
            if (_context.MyStardog.UpdateSupported)
            {
                _context.MyStardog.UpdateGraph(new Uri(_context.GRAPH_NAME), null, triples);
            }
            else
            {
                throw new Exception("Store does not support updating tripples.");
            }
        }

        public async Task DeleteAsync(string query)
        {
            _context.MyStardog.Update(query);
        }

        public Graph GetGraph()
        {
            return _context.MyGraph;
        }
    }
}
