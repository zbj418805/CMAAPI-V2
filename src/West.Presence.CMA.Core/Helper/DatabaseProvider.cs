using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Serilog;

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
        private readonly ILogger _logger = Log.ForContext<DatabaseProvider>();

        public DatabaseProvider()
        {

        }

        public T GetCellValue<T>(string connectStr, string sql, object para, CommandType type)
        {
            if (string.IsNullOrEmpty(connectStr))
            {
                _logger.Error("Conection String not set");
                return default(T);
            }

            using (var con = new SqlConnection(connectStr))
            {
                try
                {
                    return con.ExecuteScalar<T>(sql, para, null, null, type);
                }
                catch (Exception e) {
                    _logger.Error("DB Excuse Error!" + e.Message);
                    return default(T);
                }
            }
        }

        public IEnumerable<T> GetData<T>(string connectStr, string sql, object para, CommandType type)
        {
            if (string.IsNullOrEmpty(connectStr))
            {
                _logger.Error("Conection String not set");
                return null;
            }

            using (var con = new SqlConnection(connectStr))
            {
                try
                {
                    return con.Query<T>(sql, para, commandType: type) ?? new List<T>();
                }
                catch (Exception e)
                {
                    _logger.Error("DB Excuse Error!" + e.Message);
                    return null;
                }
            }
        }
        
        public void Excute(string connectStr, string sql, object para, CommandType type)
        {
            if (string.IsNullOrEmpty(connectStr))
            {
                _logger.Error("Conection String not set");
                return;
            }
            using (var con = new SqlConnection(connectStr))
            {
                try
                {
                    con.Execute(sql, para, commandType: type);
                }
                catch (Exception e)
                {
                    _logger.Error("DB Excuse Error!" + e.Message);
                }
            }
        }
    }
}
