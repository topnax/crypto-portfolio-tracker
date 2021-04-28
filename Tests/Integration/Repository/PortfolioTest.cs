using System;
using System.Security.Cryptography;
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

    public class PortfolioTest : IClassFixture<SqlKataPortfolioRepositoryFixture>
    {
        private SqlKataPortfolioRepositoryFixture _portfolioFixture;

        public PortfolioTest(SqlKataPortfolioRepositoryFixture portfolioFixture)
        {
            this._portfolioFixture = portfolioFixture;
        }

        [Fact]
        public void Add_ReturnsNonZeroId()
        {
            // arrange
            var portfolio = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet", 101);

            // act
            int id = _portfolioFixture.PortfolioRepository.Add(portfolio);

            // assert
            Assert.True(id > 0);
        }

        [Fact]
        public void Added_And_Get_AreEqual()
        {
            // arrange
            var portfolio = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet", 101);

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
        
        [Fact]
        public void AddUpdate_Updates()
        {
            // arrange
            var template = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet", 101);

            // act
            int firstId = _portfolioFixture.PortfolioRepository.Add(template);
            int secondId = _portfolioFixture.PortfolioRepository.Add(template);
            var secondPortfolio = template with
            {
                Id = secondId
            };
            var firstPortfolio = template with
            {
                // update the first entry
                Id = firstId,
                // change it's name
                Name = "Foo Portfolio"
            };
            _portfolioFixture.PortfolioRepository.Update(firstPortfolio);
            
            Assert.Equal(firstPortfolio, _portfolioFixture.PortfolioRepository.Get(firstPortfolio.Id));
            Assert.Equal(secondPortfolio, _portfolioFixture.PortfolioRepository.Get(secondPortfolio.Id));
        }

    }
}