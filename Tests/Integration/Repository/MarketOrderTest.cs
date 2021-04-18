using System;
using Database;
using Microsoft.Data.Sqlite;
using Model;
using Repository;
using SqlKata.Compilers;
using Xunit;
using Utils;

namespace Tests.Integration.Repository
{
    public class SqlKataMarketOrderRepositoryFixture : IDisposable
    {
        public SqlKataPortfolioRepository PortfolioRepository;
        public SqlKataMarketOrderRepository MarketOrderRepository;
        private SqliteConnection _dbConnection;
        public int DefaultPortfolioId;

        public SqlKataMarketOrderRepositoryFixture()
        {
            _dbConnection = new SqliteConnection("Data Source=:memory:");
            _dbConnection.Open();
            var db = new SqlKataDatabase(_dbConnection, new SqliteCompiler());
            this.PortfolioRepository = new(db);
            this.MarketOrderRepository = new(db);
            DefaultPortfolioId = PortfolioRepository.Add(new("Foo", "Bar"));
        }

        public void Dispose()
        {
            _dbConnection.Close();
        }
    }

    public class MarketOrderRepositoryTest : IClassFixture<SqlKataMarketOrderRepositoryFixture>
    {
        private SqlKataMarketOrderRepositoryFixture _marketOrderRepositoryFixture;

        public MarketOrderRepositoryTest(SqlKataMarketOrderRepositoryFixture marketOrderRepositoryFixture)
        {
            this._marketOrderRepositoryFixture = marketOrderRepositoryFixture;
        }
    }
}