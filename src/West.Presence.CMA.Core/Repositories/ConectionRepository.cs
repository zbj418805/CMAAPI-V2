using West.Presence.CMA.Core.Helper;
using Serilog;


namespace West.Presence.CMA.Core.Repositories
{
    public interface IConnectionRepository
    {
        string GetConnection(string baseUrl);
    }
    public class APIConectionRepository : IConnectionRepository
    {
        IHttpClientProvider _httpClientProvider;
        private readonly ILogger _logger = Log.ForContext<APIConectionRepository>();

        public APIConectionRepository(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public string GetConnection(string url)
        {
            return "Data Source=.;Initial Catalog=Presence_QA;User Id=sa;Password=P@ssw0rd";

            //string dbString = _httpClientProvider.GetSingleData<string>(url, "CentralServerApi");
            //if(string.IsNullOrEmpty(dbString))
            //    _logger.Error("Get db connection tring failed");
            //return dbString;
        }
    }
}
