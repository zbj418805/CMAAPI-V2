using System.Data;

namespace West.Presence.CMA.Core.Helper
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
