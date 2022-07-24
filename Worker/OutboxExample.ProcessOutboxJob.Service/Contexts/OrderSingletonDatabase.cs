using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboxExample.ProcessOutboxJob.Service.Contexts
{
    public static class OrderSingletonDatabase
    {
        static OrderSingletonDatabase()
        {
            _connection = new SqlConnection("Server=localhost, 1433;Database=OutboxExampleDB;User ID=SA;Password=1q2w3e4r+!;");
        }

        static IDbConnection _connection;
        public static IDbConnection Connection
        {
            get
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                return _connection;
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql)
            => await _connection.QueryAsync<T>(sql);
        public static async Task<int> ExecuteAsync(string sql)
            => await _connection.ExecuteAsync(sql);

        static bool _dataReaderState = true;
        public static bool DataReaderState { get => _dataReaderState; }

        public static void DataReaderReady() => _dataReaderState = true;
        public static void DataReaderBusy() => _dataReaderState = false;
    }
}
