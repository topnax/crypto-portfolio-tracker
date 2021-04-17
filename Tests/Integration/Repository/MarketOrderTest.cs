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

     //   [Fact]
     //   public void Add_ReturnsNonZeroId()
     //   {
     //       // arrange
     //       var marketOrder =
     //           new MarketOrder(Currency.Czk, 10000, 10, new Decimal(0.1), Utils.Utils.UnixTimeStampToDateTime(1618648965),
     //               true, PortfolioEntryId: _marketOrderRepositoryFixture.DefaultPortfolioId);

     //       // act
     //       int id = _marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder);
     //       var marketOrderLoaded = _marketOrderRepositoryFixture.MarketOrderRepository.Get(id);

     //       marketOrder = marketOrder with
     //       {
     //           Id = id
     //       };

     //       // assert
     //       Assert.True(id > 0);
     //       Assert.Equal(marketOrder, marketOrderLoaded);
     //   }

     //   [Fact]
     //   public void Added_And_Get_AreEqual()
     //   {
     //       // arrange
     //       var portfolio = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet");

     //       // act
     //       int id = _marketOrderRepositoryFixture.PortfolioRepository.Add(portfolio);
     //       var loaded = _marketOrderRepositoryFixture.PortfolioRepository.Get(id);
     //       portfolio = portfolio with
     //       {
     //           Id = loaded.Id
     //       };

     //       // assert
     //       Assert.True(id > 0);
     //       Assert.Equal(portfolio, loaded);
     //   }

     //   [Fact]
     //   public void AddUpdate_Updates()
     //   {
     //       // arrange
     //       var template = new Portfolio("My new portfolio", "Lorem ipsum dolor sit amet");

     //       // act
     //       int firstId = _marketOrderRepositoryFixture.PortfolioRepository.Add(template);
     //       int secondId = _marketOrderRepositoryFixture.PortfolioRepository.Add(template);
     //       var secondPortfolio = template with
     //       {
     //           Id = secondId
     //       };
     //       var firstPortfolio = template with
     //       {
     //           // update the first entry
     //           Id = firstId,
     //           // change it's name
     //           Name = "Foo Portfolio"
     //       };
     //       _marketOrderRepositoryFixture.PortfolioRepository.Update(firstPortfolio);

     //       Assert.Equal(firstPortfolio, _marketOrderRepositoryFixture.PortfolioRepository.Get(firstPortfolio.Id));
     //       Assert.Equal(secondPortfolio, _marketOrderRepositoryFixture.PortfolioRepository.Get(secondPortfolio.Id));
     //   }
    }
}