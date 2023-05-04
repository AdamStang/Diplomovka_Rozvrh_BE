using VDS.RDF;
using VDS.RDF.Storage;

namespace backend.Database
{
    public interface IDbContext
    {
        public static string BASE_URI { get; }
        public string GRAPH_NAME { init; get; }
        public StardogConnector MyStardog { init; get; }
        public Graph MyGraph { init; get; }
    }
}
