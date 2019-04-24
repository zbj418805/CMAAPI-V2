using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace West.Presence.CMA.Core.Helper
{

    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    
    public interface IDatabaseProvider
    {
        T GetCellValue<T>(string sql, object para);
        IEnumerable<T> GetData<T>(string sql, object para, CommandType type);
    }

    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DatabaseProvider(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public T GetCellValue<T>(string sql, object para)
        {
            using(var con = _dbConnectionFactory.CreateConnection())
            {
                return con.ExecuteScalar<T>(sql, para);
            }
        }

        public IEnumerable<T> GetData<T>(string sql, object para, CommandType type)
        {
            using (var con = _dbConnectionFactory.CreateConnection())
            {
                return con.Query<T>(sql, para, commandType: type) ?? new List<T>();
            }
        }  
    }
}
