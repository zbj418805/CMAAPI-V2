using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Dapper;

namespace West.Presence.CMA.Core.Helper
{
    public interface IDatabaseProvider
    {
        T GetCellValue<T>(string connectStr, string sql, object para, CommandType type);
        IEnumerable<T> GetData<T>(string connectStr, string sql, object para, CommandType type);
        void Excute(string connectStr, string sql, object para, CommandType type);
    }

    public class DatabaseProvider : IDatabaseProvider
    {
        public DatabaseProvider()
        {

        }

        public T GetCellValue<T>(string connectStr, string sql, object para, CommandType type)
        {
            using (var con = new SqlConnection(connectStr))
            {
                return con.ExecuteScalar<T>(sql, para, null, null, type);
            }
        }

        public IEnumerable<T> GetData<T>(string connectStr, string sql, object para, CommandType type)
        {
            using (var con = new SqlConnection(connectStr))
            {
                return con.Query<T>(sql, para, commandType: type) ?? new List<T>();
            }
        }
        
        public void Excute(string connectStr, string sql, object para, CommandType type)
        {
            using (var con = new SqlConnection(connectStr))
            {
                con.Execute(sql, para, commandType: type);
            }
        }
    }
}
