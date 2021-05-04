using CryptoStatsSource;
using Xunit;

namespace Tests.Integration.CryptoStatsSource
{
    public class GetMarketEntriesTest
    {
        private readonly CoingeckoSource _source = new();

        [Fact]
        public async void SimpleThreeEntries()
        {
            var entries = await _source.GetMarketEntries("usd", "bitcoin", "litecoin", "cardano");

            Assert.Equal(3, entries.Count);
            Assert.True(entries.Find(entry => entry.Symbol == "btc") != null);
            Assert.True(entries.Find(entry => entry.Symbol == "ltc") != null);
            Assert.True(entries.Find(entry => entry.Symbol == "ada") != null);
        }

        [Fact]
        public async void SimpleOneEntry()
        {
            var entries = await _source.GetMarketEntries("usd", "bitcoin");

            Assert.Single(entries);
            Assert.Equal("btc", entries[0].Symbol);
            // 🙏🏻 
            Assert.True(entries[0].CurrentPrice > 10000);
            Assert.Equal("btc", entries[0].Symbol);
            Assert.Equal("Bitcoin", entries[0].Name);
        }
    }
}