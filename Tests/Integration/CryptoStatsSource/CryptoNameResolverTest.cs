using CryptoStatsSource;
using Xunit;

namespace Tests.Integration.CryptoStatsSource
{
    public class ResolveNameTest
    {
        private readonly CoingeckoSource _source;
        private readonly CryptoNameResolverImpl _resolver;

        public ResolveNameTest()
        {
            _source = new();
            _resolver = new(_source);
        }

        [Fact]
        public async void SimpleThreeEntries()
        {
            Assert.Equal("Bitcoin", await _resolver.Resolve("btc"));
            Assert.Equal("Cardano", await _resolver.Resolve("ada"));
            Assert.Equal("Litecoin", await _resolver.Resolve("ltc"));
            Assert.Equal("Ethereum", await _resolver.Resolve("eth"));
            Assert.Null(await _resolver.Resolve("abcefghbzbzrfoo"));
        }
    }
}