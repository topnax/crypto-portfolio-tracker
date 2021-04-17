using System;
using Database;
using Microsoft.Data.Sqlite;
using Model;
using Repository;
using SqlKata.Compilers;
using Xunit;

namespace Tests.Integration.Repository
{
    public class SqlKataPortfolioRepositoryFixture : IDisposable
    {
        public SqlKataPortfolioRepository PortfolioRepository;
        private SqliteConnection _dbConnection;

        public SqlKataPortfolioRepositoryFixture()
        {
            _dbConnection = new SqliteConnection("Data Source=:memory:");
            _dbConnection.Open();
            var db = new SqlKataDatabase(_dbConnection, new SqliteCompiler());
            this.PortfolioRepository = new(db);
        }

        public void Dispose()
        {
            _dbConnection.Close();
        }
    }

    public class UnitTest1 : IClassFixture<SqlKataPortfolioRepositoryFixture>
    {
        private SqlKataPortfolioRepositoryFixture _portfolioFixture;

        public UnitTest1(SqlKataPortfolioRepositoryFixture portfolioFixture)
        {
            this._portfolioFixture = portfolioFixture;
        }

        [Fact]
        public void Add_ReturnsNonZeroId()
        {
            // arrange
            var portfolio = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet");

            // act
            int id = _portfolioFixture.PortfolioRepository.Add(new Portfolio("My new portfolio",
                "Lorem ipsum dolor sit amet"));

            // assert
            Assert.True(id > 0);
        }

        [Fact]
        public void Added_And_Get_AreEqual()
        {
            // arrange
            var portfolio = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet");

            // act
            int id = _portfolioFixture.PortfolioRepository.Add(portfolio);
            var loaded = _portfolioFixture.PortfolioRepository.Get(id);
            portfolio = portfolio with
            {
                Id = loaded.Id
            };

            // assert
            Assert.True(id > 0);
            Assert.Equal(portfolio, loaded);
        }
    }
}