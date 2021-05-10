using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoStatsSource.model;

namespace CryptoStatsSource
{
    public interface ICryptocurrencyResolver
    {
        public Task<Cryptocurrency> Resolve(string symbol);

        public Task Refresh();
    }

    public class CryptocurrencyResolverImpl : ICryptocurrencyResolver
    {
        private ICryptoStatsSource _cryptoStatsSource;
        private Dictionary<String, Cryptocurrency> _nameToCryptocurrencyDictionary;

        public CryptocurrencyResolverImpl(ICryptoStatsSource cryptoStatsSource)
        {
            _cryptoStatsSource = cryptoStatsSource;
        }

        public async Task Refresh()
        {
            // TODO improve this
            _nameToCryptocurrencyDictionary = new();
            (await _cryptoStatsSource.GetAvailableCryptocurrencies()).ForEach(c =>
                _nameToCryptocurrencyDictionary.TryAdd(c.Symbol, c));
        }

        public async Task<Cryptocurrency> Resolve(string symbol)
        {
            if (_nameToCryptocurrencyDictionary?.GetValueOrDefault(symbol) == null) await Refresh();

            return _nameToCryptocurrencyDictionary.GetValueOrDefault(symbol, null);
        }
    }
}