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
            Assert.Equal(new("bitcoin", "btc", "Bitcoin"), await _resolver.Resolve("btc"));
            Assert.Equal(new ("cardano", "ada", "Cardano"), await _resolver.Resolve("cardano"));
            Assert.Equal(new ("litecoin", "ltc", "Litecoin"), await _resolver.Resolve("ltc"));
            Assert.Equal(new("ethereum", "eth", "Ethereum"), await _resolver.Resolve("eth"));
            Assert.Null(await _resolver.Resolve("abcefghbzbzrfoo"));
        }
    }
}