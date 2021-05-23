using System;
using System.Collections.Generic;
using Database;
using Microsoft.Data.Sqlite;
using Model;
using Repository;
using SqlKata.Compilers;
using Xunit;

namespace Tests.Integration.Repository
{
    public class SqlKataMarketOrderRepositoryFixture : IDisposable
    {
        public SqlKataMarketOrderRepository MarketOrderRepository;
        private SqliteConnection _dbConnection;
        public int DefaultPortfolioEntryId;
        public int SecondaryPortfolioEntryId;

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
            SecondaryPortfolioEntryId = portfolioEntryRepository.Add(new("ltc", defaultPortfolioId));
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
            var marketOrder2 = new MarketOrder(new Decimal(11000.39), 11, new Decimal(1.2),
                DateTime.Now.Subtract(TimeSpan.FromSeconds(3600)), true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);
            var marketOrder3 = new MarketOrder(new Decimal(12000.39), 12, new Decimal(1.3),
                DateTime.Now.Subtract(TimeSpan.FromDays(30)), false,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);

            // act
            marketOrder1 = marketOrder1 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder1)
            };
            marketOrder2 = marketOrder2 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder2)
            };
            marketOrder3 = marketOrder3 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder3)
            };

            // assert
            var loadedPortfolios = marketOrderRepositoryFixture.MarketOrderRepository.GetAll();
            Assert.Equal(3, loadedPortfolios.Count);
            Assert.Equal(new List<MarketOrder> {marketOrder1, marketOrder2, marketOrder3}, loadedPortfolios);
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

        [Fact]
        public void GetAllByPortfolioEntry_Returns_Correct_Orders()
        {
            // fixture unique to this test
            var marketOrderRepositoryFixture = new SqlKataMarketOrderRepositoryFixture();

            // arrange
            var marketOrder1 = new MarketOrder(new Decimal(10000.39), 10, new Decimal(1.1), DateTime.Now, true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);
            var marketOrder2 = new MarketOrder(new Decimal(11000.39), 11, new Decimal(1.2),
                DateTime.Now.Subtract(TimeSpan.FromSeconds(3600)), true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);
            var marketOrder3 = new MarketOrder(new Decimal(12000.39), 12, new Decimal(1.3),
                DateTime.Now.Subtract(TimeSpan.FromDays(30)), false,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);

            var marketOrder4 = new MarketOrder(new Decimal(12005.39), 15, new Decimal(12),
                DateTime.Now.Subtract(TimeSpan.FromDays(11)), false,
                PortfolioEntryId: marketOrderRepositoryFixture.SecondaryPortfolioEntryId);

            var marketOrder5 = new MarketOrder(new Decimal(12006.39), 16, new Decimal(1.5),
                DateTime.Now.Subtract(TimeSpan.FromDays(39)), false,
                PortfolioEntryId: marketOrderRepositoryFixture.SecondaryPortfolioEntryId);

            // act
            var presumablyEmptyList =
                marketOrderRepositoryFixture.MarketOrderRepository.GetAllByPortfolioEntryId(marketOrderRepositoryFixture
                    .DefaultPortfolioEntryId);
            var presumablyEmptyList2 =
                marketOrderRepositoryFixture.MarketOrderRepository.GetAllByPortfolioEntryId(
                    marketOrderRepositoryFixture.SecondaryPortfolioEntryId);

            marketOrder1 = marketOrder1 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder1)
            };
            marketOrder2 = marketOrder2 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder2)
            };
            marketOrder3 = marketOrder3 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder3)
            };

            marketOrder4 = marketOrder4 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder4)
            };
            marketOrder5 = marketOrder5 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder5)
            };

            // assert
            var loadedPortfolios =
                marketOrderRepositoryFixture.MarketOrderRepository.GetAllByPortfolioEntryId(marketOrderRepositoryFixture
                    .DefaultPortfolioEntryId);
            Assert.Empty(presumablyEmptyList);
            Assert.Empty(presumablyEmptyList2);

            Assert.Equal(3, loadedPortfolios.Count);
            Assert.Equal(new List<MarketOrder> {marketOrder1, marketOrder2, marketOrder3}, loadedPortfolios);

            var loadedPortfoliosSecondary =
                marketOrderRepositoryFixture.MarketOrderRepository.GetAllByPortfolioEntryId(marketOrderRepositoryFixture
                    .SecondaryPortfolioEntryId);
            Assert.Equal(2, loadedPortfoliosSecondary.Count);
            Assert.Equal(new List<MarketOrder> {marketOrder4, marketOrder5}, loadedPortfoliosSecondary);
        }

        [Fact]
        public void Delete_Deletes()
        {
            // fixture unique to this test
            var marketOrderRepositoryFixture = new SqlKataMarketOrderRepositoryFixture();

            // arrange
            var marketOrder1 = new MarketOrder(new Decimal(10000.39), 10, new Decimal(1.1), DateTime.Now, true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);
            var marketOrder2 = new MarketOrder(new Decimal(11000.39), 11, new Decimal(1.2),
                DateTime.Now.Subtract(TimeSpan.FromSeconds(3600)), true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);


            // act
            marketOrder1 = marketOrder1 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder1)
            };
            marketOrder2 = marketOrder2 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder2)
            };

            marketOrderRepositoryFixture.MarketOrderRepository.Delete(marketOrder1);

            // assert
            Assert.Null(marketOrderRepositoryFixture.MarketOrderRepository.Get(marketOrder1.Id));
            Assert.Equal(marketOrder2, marketOrderRepositoryFixture.MarketOrderRepository.Get(marketOrder2.Id));
            Assert.Single(marketOrderRepositoryFixture.MarketOrderRepository.GetAll());
        }

        [Fact]
        public void DeleteEntryOrders_Deletes_All_Orders()
        {
            // fixture unique to this test
            var marketOrderRepositoryFixture = new SqlKataMarketOrderRepositoryFixture();

            // arrange
            var marketOrder1 = new MarketOrder(new Decimal(10000.39), 10, new Decimal(1.1), DateTime.Now, true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);
            var marketOrder2 = new MarketOrder(new Decimal(11000.39), 11, new Decimal(1.2),
                DateTime.Now.Subtract(TimeSpan.FromSeconds(3600)), true,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);
            var marketOrder3 = new MarketOrder(new Decimal(12000.39), 12, new Decimal(1.3),
                DateTime.Now.Subtract(TimeSpan.FromDays(30)), false,
                PortfolioEntryId: marketOrderRepositoryFixture.DefaultPortfolioEntryId);

            var marketOrder4 = new MarketOrder(new Decimal(12005.39), 15, new Decimal(12),
                DateTime.Now.Subtract(TimeSpan.FromDays(11)), false,
                PortfolioEntryId: marketOrderRepositoryFixture.SecondaryPortfolioEntryId);

            var marketOrder5 = new MarketOrder(new Decimal(12006.39), 16, new Decimal(1.5),
                DateTime.Now.Subtract(TimeSpan.FromDays(39)), false,
                PortfolioEntryId: marketOrderRepositoryFixture.SecondaryPortfolioEntryId);

            // act
            marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder1);
            marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder2);
            marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder3);

            marketOrder4 = marketOrder4 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder4)
            };
            marketOrder5 = marketOrder5 with
            {
                Id = marketOrderRepositoryFixture.MarketOrderRepository.Add(marketOrder5)
            };

            marketOrderRepositoryFixture.MarketOrderRepository.DeletePortfolioEntryOrders(marketOrderRepositoryFixture
                .DefaultPortfolioEntryId);

            // assert
            var loadedPortfolios =
                marketOrderRepositoryFixture.MarketOrderRepository.GetAllByPortfolioEntryId(marketOrderRepositoryFixture
                    .DefaultPortfolioEntryId);

            Assert.Empty(loadedPortfolios);

            var loadedPortfoliosSecondary =
                marketOrderRepositoryFixture.MarketOrderRepository.GetAllByPortfolioEntryId(marketOrderRepositoryFixture
                    .SecondaryPortfolioEntryId);
            Assert.Equal(2, loadedPortfoliosSecondary.Count);
            Assert.Equal(new List<MarketOrder> {marketOrder4, marketOrder5}, loadedPortfoliosSecondary);
        }
    }
}