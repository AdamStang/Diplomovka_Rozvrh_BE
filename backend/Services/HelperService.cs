using backend.Database;

namespace backend.Services
{
    public class HelperService
    {
        public static Uri CreateUri(string param) 
        {
            return new Uri($"{DbConstants.BASE_URI}{param}");
        }
    }
}
