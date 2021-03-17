using System;
using System.Data;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Database
{
    public class SqlKataDatabase : IDisposable
    {
        private readonly QueryFactory _queryFactory;
        private readonly IDbConnection _dbConnection;

        public SqlKataDatabase(IDbConnection dbConnection, Compiler compiler)
        {
            _dbConnection = dbConnection;
            _queryFactory = new(dbConnection, compiler);

            Console.WriteLine("SqlKataDatabase constructor invoked");
        }

        public QueryFactory Get() => _queryFactory;

        public void Dispose()
        {
            Console.WriteLine("Disposing SqlKataDatabase");
            _dbConnection.Close();
        }
    }
}