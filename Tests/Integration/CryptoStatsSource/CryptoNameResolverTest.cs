using CryptoStatsSource;
using Xunit;

namespace Tests.Integration.CryptoStatsSource
{
    public class ResolveNameTest
    {
        private readonly CoingeckoSource _source;
        private readonly CryptocurrencyResolverImpl _resolver;

        public ResolveNameTest()
        {
            _source = new();
            _resolver = new(_source);
        }

        [Fact]
        public async void SimpleThreeEntries()
        {
            Assert.Equal(new("btc", "btc", "Bitcoin"), await _resolver.Resolve("btc"));
            Assert.Equal(new ("ada", "ada", "Cardano"), await _resolver.Resolve("ada"));
            Assert.Equal(new ("ltc", "ltc", "Litecoin"), await _resolver.Resolve("ltc"));
            Assert.Equal(new("eth", "eth", "Ethereum"), await _resolver.Resolve("eth"));
            Assert.Null(await _resolver.Resolve("abcefghbzbzrfoo"));
        }
    }
}