using System;
using System.Collections.Generic;
using Database;
using Microsoft.Data.Sqlite;
using Model;
using Repository;
using SqlKata.Compilers;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Integration.Repository
{
    public class SqlKataMarketOrderRepositoryFixture : IDisposable
    {
        public SqlKataMarketOrderRepository MarketOrderRepository;
        private SqliteConnection _dbConnection;
        public int DefaultPortfolioEntryId;

        public SqlKataMarketOrderRepositoryFixture()
        {
            _dbConnection = new SqliteConnection("Data Source=:memory:");
            _dbConnection.Open();
            var db = new SqlKataDatabase(_dbConnection, new SqliteCompiler());
            SqlKataPortfolioRepository portfolioRepository = new(db);
            SqlKataPortfolioEntryRepository portfolioEntryRepository = new(db);
            this.MarketOrderRepository = new(db);
            var defaultPortfolioId = portfolioRepository.Add(new("Foo", "Bar", Currency.Eur));
            DefaultPortfolioEntryId = portfolioEntryRepository.Add(new("btc", defaultPortfolioId));
        }

        public void Dispose()
        {
            _dbConnection.Close();
        }
    }

    public class MarketOrderRepositoryTest : IClassFixture<SqlKataMarketOrderRepositoryFixture>
    {
        private SqlKataMarketOrderRepositoryFixture _marketOrderRepositoryFixture;

        // TODO test Delete method
        public MarketOrderRepositoryTest(SqlKataMarketOrderRepositoryFixture marketOrderRepositoryFixture,
            ITestOutputHelper testOutputHelper)
        {
            this._marketOrderRepositoryFixture = marketOrderRepositoryFixture;
        }

        [Fact]
        public void Add_ReturnsNonZeroId()
        {
            // arrange
            var marketOrder = new MarketOrder(new Decimal(10000.39), 10, new Decimal(1.1), DateTime.Now, true,
                PortfolioEntryId: _marketOrderRepositoryFixture.DefaultPortfolioEntryId);

            // act
            int id = _marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder);

            // assert
            Assert.True(id > 0);
        }


        [Fact]
        public void Added_And_Get_AreEqual()
        {
            // arrange
            var marketOrder = new MarketOrder(new Decimal(10000.39), 10, new Decimal(1.1), DateTime.Now, true,
                PortfolioEntryId: _marketOrderRepositoryFixture.DefaultPortfolioEntryId);

            // act
            int id = _marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder);
            // update the added order with the generated id
            marketOrder = marketOrder with
            {
                Id = id
            };

            // assert
            Assert.True(id > 0);
            var actual = _marketOrderRepositoryFixture.MarketOrderRepository.Get(marketOrder.Id);
            Assert.Equal(marketOrder, actual);
        }

        [Fact]
        public void Added_And_GetAll_AreEqual()
        {
            // fixture unique to this test
            var marketOrderRepositoryFixture = new SqlKataMarketOrderRepositoryFixture();
            
            // arrange
            var marketOrder1 = new MarketOrder(new Decimal(10000.39), 10, new Decimal(1.1), DateTime.Now, true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);
            var marketOrder2 = new MarketOrder(new Decimal(11000.39), 11, new Decimal(1.2), DateTime.Now.Subtract(TimeSpan.FromSeconds(3600)), true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);
            var marketOrder3 = new MarketOrder(new Decimal(12000.39), 12, new Decimal(1.3), DateTime.Now.Subtract(TimeSpan.FromDays(30)), false,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);

            // act
            marketOrder1 = marketOrder1 with {Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder1)};
            marketOrder2 = marketOrder2 with {Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder2)};
            marketOrder3 = marketOrder3 with {Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder3)};
            
            // assert
            var loadedPortfolios = marketOrderRepositoryFixture.MarketOrderRepository.GetAll();
            Assert.Equal(3, loadedPortfolios.Count);
            Assert.Equal(new List<MarketOrder>{marketOrder1, marketOrder2, marketOrder3}, loadedPortfolios);
        }
        
        [Fact]
        public void AddUpdate_Updates()
        {
            // arrange
            var marketOrder = new MarketOrder(new Decimal(10000.39), 10, new Decimal(1.1), DateTime.Now, true,
                PortfolioEntryId: _marketOrderRepositoryFixture.DefaultPortfolioEntryId);

            // act
            int id = _marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder);
            // update the added order with the generated id and change the filled price, size and buy flag
            marketOrder = marketOrder with
            {
                Id = id,
                FilledPrice = 11001,
                Size = new decimal(1.3),
                Buy = false
            };
            _marketOrderRepositoryFixture.MarketOrderRepository.Update(marketOrder);

            // assert
            Assert.Equal(marketOrder, _marketOrderRepositoryFixture.MarketOrderRepository.Get(id));
        }
    }
}