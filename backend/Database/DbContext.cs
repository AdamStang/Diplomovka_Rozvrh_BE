using VDS.RDF;
using VDS.RDF.Storage;

namespace backend.Database
{
    public class DbContext: IDbContext
    {
        const string SERVER_URL = "https://sd-ea75aefd.stardog.cloud:5820";
        const string USERNAME = "Adam";
        const string PASSWORD = "sWTCRSvdxLEF545";
        const string DB_NAME = "Timetable";


        public DbContext() 
        {
            MyGraph.BaseUri = new Uri(BASE_URI);
        }

        public static string BASE_URI { get; } = "http://fei.stuba.sk/";
        public StardogConnector MyStardog { get; init; } = new StardogConnector(SERVER_URL, DB_NAME, USERNAME, PASSWORD);
        public Graph MyGraph { get; init; } = new Graph();
        public string GRAPH_NAME { get; init; } = "timetable:stuba:fei";

    }
}
