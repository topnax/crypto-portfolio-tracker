using System;
using System.Collections.Generic;
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
        private SqlKataPortfolioRepositoryFixture _portfolioRepositoryFixture;

        public PortfolioTest(SqlKataPortfolioRepositoryFixture portfolioRepositoryFixture)
        {
            this._portfolioRepositoryFixture = portfolioRepositoryFixture;
        }

        [Fact]
        public void Add_ReturnsNonZeroId()
        {
            // arrange
            var portfolio = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet", Currency.Eur);

            // act
            int id = _portfolioRepositoryFixture.PortfolioRepository.Add(portfolio);

            // assert
            Assert.True(id > 0);
        }

        [Fact]
        public void Added_And_Get_AreEqual()
        {
            // arrange
            var portfolio = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet", Currency.Czk);

            // act
            int id = _portfolioRepositoryFixture.PortfolioRepository.Add(portfolio);
            var loaded = _portfolioRepositoryFixture.PortfolioRepository.Get(id);
            portfolio = portfolio with
            {
                Id = loaded.Id
            };

            // assert
            Assert.True(id > 0);
            Assert.Equal(portfolio, loaded);
        }

        [Fact]
        public void Added_And_GetAll_AreEqual()
        {
            // fixture unique to this test
            var portfolioRepositoryFixture = new SqlKataPortfolioRepositoryFixture();

            // arrange
            var portfolio1 = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet", Currency.Czk);
            var portfolio2 = new Portfolio("My second portfolio", "Lorem ipsum dolor sit amet", Currency.Eur);
            var portfolio3 = new Portfolio("My third portfolio", "Lorem ipsum dolor sit amet", Currency.Usd);

            // act
            portfolio1 = portfolio1 with {Id = portfolioRepositoryFixture.PortfolioRepository.Add(portfolio1)};
            portfolio2 = portfolio2 with {Id = portfolioRepositoryFixture.PortfolioRepository.Add(portfolio2)};
            portfolio3 = portfolio3 with {Id = portfolioRepositoryFixture.PortfolioRepository.Add(portfolio3)};

            // assert
            var loadedPortfolios = portfolioRepositoryFixture.PortfolioRepository.GetAll();
            Assert.Equal(3, loadedPortfolios.Count);
            Assert.Equal(new List<Portfolio> {portfolio1, portfolio2, portfolio3}, loadedPortfolios);
        }

        [Fact]
        public void AddUpdate_Updates()
        {
            // arrange
            var template = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet", Currency.Usd);

            // act
            int firstId = _portfolioRepositoryFixture.PortfolioRepository.Add(template);
            int secondId = _portfolioRepositoryFixture.PortfolioRepository.Add(template);
            var secondPortfolio = template with
            {
                Id = secondId
            };
            var firstPortfolio = template with
            {
                // update the first entry
                Id = firstId,
                // change it's name
                Name = "Foo Portfolio",
                Currency = Currency.Eur
            };
            _portfolioRepositoryFixture.PortfolioRepository.Update(firstPortfolio);

            Assert.Equal(firstPortfolio, _portfolioRepositoryFixture.PortfolioRepository.Get(firstPortfolio.Id));
            Assert.Equal(secondPortfolio, _portfolioRepositoryFixture.PortfolioRepository.Get(secondPortfolio.Id));
        }

        [Fact]
        public void Delete_Deletes()
        {
            // fixture unique to this test
            var portfolioRepositoryFixture = new SqlKataPortfolioRepositoryFixture();
            
            // arrange
            var firstPortfolio = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet", Currency.Usd);
            var secondPortfolio = new Portfolio("My second new portfolio", "Lorem ipsum dolor sit amet", Currency.Eur);

            // act
            firstPortfolio = firstPortfolio with
            {
                Id = portfolioRepositoryFixture.PortfolioRepository.Add(firstPortfolio)
            };
            secondPortfolio = secondPortfolio with
            {
                Id = portfolioRepositoryFixture.PortfolioRepository.Add(secondPortfolio)
            };
            portfolioRepositoryFixture.PortfolioRepository.Delete(firstPortfolio);

            // assert
            Assert.Null(portfolioRepositoryFixture.PortfolioRepository.Get(firstPortfolio.Id));
            Assert.Equal(secondPortfolio, portfolioRepositoryFixture.PortfolioRepository.Get(secondPortfolio.Id));
            Assert.Equal(1, portfolioRepositoryFixture.PortfolioRepository.GetAll().Count);
        }
    }
}