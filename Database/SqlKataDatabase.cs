using System;
using System.Data;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Database
{
    /**
     * Wraps over a SQL database connection and adds SqlKata extensions via QueryFactory
     */
    public class SqlKataDatabase : IDisposable
    {
        private readonly QueryFactory _queryFactory;
        private readonly IDbConnection _dbConnection;

        public SqlKataDatabase(IDbConnection dbConnection, Compiler compiler)
        {
            _dbConnection = dbConnection;
            _queryFactory = new(dbConnection, compiler);
            
            Console.WriteLine("SqlKataDatabase constructor invoked");
            SqlSchema.Init(this);
        }

        public QueryFactory Get() => _queryFactory;

        public void Dispose()
        {
            // TODO check whether dispose is called
            Console.WriteLine("Disposing SqlKataDatabase");
            _dbConnection.Close();
        }
    }
}