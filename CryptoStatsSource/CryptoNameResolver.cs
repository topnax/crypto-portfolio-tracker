using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoStatsSource
{
    public interface ICryptoNameResolver
    {
        public Task<string> Resolve(string symbol);

        public Task Refresh();
    }

    public class CryptoNameResolverImpl : ICryptoNameResolver
    {
        private ICryptoStatsSource _cryptoStatsSource;
        private Dictionary<String, String> _symbolToNameMap;

        public CryptoNameResolverImpl(ICryptoStatsSource cryptoStatsSource)
        {
            _cryptoStatsSource = cryptoStatsSource;
        }

        public async Task Refresh()
        {
            _symbolToNameMap = new();
            (await _cryptoStatsSource.GetAvailableCryptocurrencies()).ForEach(c =>
                _symbolToNameMap.TryAdd(c.Symbol, c.Name));
        }


        public async Task<string> Resolve(string symbol)
        {
            if (_symbolToNameMap?.GetValueOrDefault(symbol) == null) await Refresh();

            return _symbolToNameMap.GetValueOrDefault(symbol, null);
        }
    }
}