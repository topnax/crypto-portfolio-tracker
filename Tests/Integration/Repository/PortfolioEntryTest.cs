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
    public class SqlKataPortfolioEntryRepositoryFixture : IDisposable
    {
        public SqlKataPortfolioRepository PortfolioRepository;
        public SqlKataPortfolioEntryRepository PortfolioEntryRepository;
        private SqliteConnection _dbConnection;
        public int DefaultPortfolioId;
        public int SecondaryPortfolioId;

        public SqlKataPortfolioEntryRepositoryFixture()
        {
            _dbConnection = new SqliteConnection("Data Source=:memory:");
            _dbConnection.Open();
            var db = new SqlKataDatabase(_dbConnection, new SqliteCompiler());
            this.PortfolioRepository = new(db);
            this.PortfolioEntryRepository = new(db);
            DefaultPortfolioId = PortfolioRepository.Add(new("Foo", "Bar", Currency.Czk));
            SecondaryPortfolioId = PortfolioRepository.Add(new("Bar", "Bar", Currency.Czk));
        }

        public void Dispose()
        {
            _dbConnection.Close();
        }
    }

    public class PortfolioEntryRepositoryTest : IClassFixture<SqlKataPortfolioEntryRepositoryFixture>
    {
        private SqlKataPortfolioEntryRepositoryFixture _portfolioEntryRepositoryFixture;

        // TODO test Delete method
        public PortfolioEntryRepositoryTest(SqlKataPortfolioEntryRepositoryFixture marketOrderRepositoryFixture)
        {
            this._portfolioEntryRepositoryFixture = marketOrderRepositoryFixture;
        }

        [Fact]
        public void Add_ReturnsNonZeroId()
        {
            // arrange
            var portfolioEntry = new PortfolioEntry("btc", _portfolioEntryRepositoryFixture.DefaultPortfolioId);

            // act
            int id = _portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry);

            // assert
            Assert.True(id > 0);
        }

        [Fact]
        public void Added_And_Get_AreEqual()
        {
            // arrange
            var portfolioEntry = new PortfolioEntry("btc", _portfolioEntryRepositoryFixture.DefaultPortfolioId);

            // act
            int id = _portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry);

            portfolioEntry = portfolioEntry with
            {
                Id = id
            };

            // assert
            Assert.True(id > 0);
            Assert.Equal(portfolioEntry,
                _portfolioEntryRepositoryFixture.PortfolioEntryRepository.Get(portfolioEntry.Id));
        }

        [Fact]
        public void Added_And_GetAll_AreEqual()
        {
            // fixture unique to this test
            var portfolioEntryRepositoryFixture = new SqlKataPortfolioEntryRepositoryFixture();

            // arrange
            var portfolioEntry1 = new PortfolioEntry("btc", portfolioEntryRepositoryFixture.DefaultPortfolioId);
            var portfolioEntry2 = new PortfolioEntry("ada", portfolioEntryRepositoryFixture.DefaultPortfolioId);
            var portfolioEntry3 = new PortfolioEntry("ltc", portfolioEntryRepositoryFixture.DefaultPortfolioId);

            // act
            portfolioEntry1 = portfolioEntry1 with
            {
                Id = portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry1)
            };
            portfolioEntry2 = portfolioEntry2 with
            {
                Id = portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry2)
            };
            portfolioEntry3 = portfolioEntry3 with
            {
                Id = portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry3)
            };

            // assert
            var loadedPortfolios = portfolioEntryRepositoryFixture.PortfolioEntryRepository.GetAll();
            Assert.Equal(3, loadedPortfolios.Count);
            Assert.Equal(new List<PortfolioEntry> {portfolioEntry1, portfolioEntry2, portfolioEntry3},
                loadedPortfolios);
        }

        [Fact]
        public void GetAllByPortfolioId_Returns_Correct_Entries()
        {
            // fixture unique to this test
            var portfolioEntryRepositoryFixture = new SqlKataPortfolioEntryRepositoryFixture();

            // arrange
            var portfolioEntry1 = new PortfolioEntry("btc", portfolioEntryRepositoryFixture.DefaultPortfolioId);
            var portfolioEntry2 = new PortfolioEntry("ada", portfolioEntryRepositoryFixture.DefaultPortfolioId);
            var portfolioEntry3 = new PortfolioEntry("ltc", portfolioEntryRepositoryFixture.DefaultPortfolioId);
            
            var portfolioEntry4 = new PortfolioEntry("btc", portfolioEntryRepositoryFixture.SecondaryPortfolioId);
            var portfolioEntry5 = new PortfolioEntry("eth", portfolioEntryRepositoryFixture.SecondaryPortfolioId);

            // act
            portfolioEntry1 = portfolioEntry1 with
            {
                Id = portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry1)
            };
            portfolioEntry2 = portfolioEntry2 with
            {
                Id = portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry2)
            };
            portfolioEntry3 = portfolioEntry3 with
            {
                Id = portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry3)
            };
            
            portfolioEntry4 = portfolioEntry4 with
            {
                Id = portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry4)
            };
            portfolioEntry5 = portfolioEntry5 with
            {
                Id = portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(portfolioEntry5)
            };

            // assert
            var loadedPortfolios = portfolioEntryRepositoryFixture.PortfolioEntryRepository.GetAllByPortfolioId(portfolioEntryRepositoryFixture.DefaultPortfolioId);
            Assert.Equal(3, loadedPortfolios.Count);
            Assert.Equal(new List<PortfolioEntry> {portfolioEntry1, portfolioEntry2, portfolioEntry3},
                loadedPortfolios);
            
            var loadedPortfoliosSecondaryPortfolio = portfolioEntryRepositoryFixture.PortfolioEntryRepository.GetAllByPortfolioId(portfolioEntryRepositoryFixture.SecondaryPortfolioId);
            Assert.Equal(2, loadedPortfoliosSecondaryPortfolio.Count);
            Assert.Equal(new List<PortfolioEntry> {portfolioEntry4, portfolioEntry5},
                loadedPortfoliosSecondaryPortfolio);
        }

        [Fact]
        public void AddUpdate_Updates()
        {
            // arrange
            var btcEntry = new PortfolioEntry("btc", _portfolioEntryRepositoryFixture.DefaultPortfolioId);
            var ethEntry = new PortfolioEntry("eth", _portfolioEntryRepositoryFixture.DefaultPortfolioId);

            // act
            int btcId = _portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(btcEntry);
            int ethId = _portfolioEntryRepositoryFixture.PortfolioEntryRepository.Add(ethEntry);
            ethEntry = ethEntry with
            {
                Id = ethId
            };
            btcEntry = btcEntry with
            {
                // update the first entry
                Id = btcId,
                // change it's name
                Symbol = "ltc"
            };
            _portfolioEntryRepositoryFixture.PortfolioEntryRepository.Update(btcEntry);

            Assert.Equal(btcEntry, _portfolioEntryRepositoryFixture.PortfolioEntryRepository.Get(btcEntry.Id));
            Assert.Equal(ethEntry, _portfolioEntryRepositoryFixture.PortfolioEntryRepository.Get(ethEntry.Id));
        }
    }
}