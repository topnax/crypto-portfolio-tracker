using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoStatsSource.model;

namespace CryptoStatsSource
{
    public interface ICryptocurrencyResolver
    {
        /// <summary>
        /// Resolves a cryptocurrency based on the given cryptocurrency symbol. Returns null value when the symbol does
        /// not map to any cryptocurrency.
        /// </summary>
        /// <param name="symbol">Symbol based on which the cryptocurrency should be resolved</param>
        /// <returns>A task containing resolved cryptocurrency when finished</returns>
        public Task<Cryptocurrency> Resolve(string symbol);

        /// <summary>
        /// Refreshes the database of cryptocurrencies mapped to their symbols
        /// </summary>
        /// <returns>A task that is finished when all cryptocurrencies are fetched and mapped to their symbols</returns>
        public Task Refresh();
    }

    public class CryptocurrencyResolverImpl : ICryptocurrencyResolver
    {
        // used for retrieving cryptocurrency info
        private ICryptoStatsSource _cryptoStatsSource;
        
        // a dictionary mapping symbols to cryptocurrencies
        private Dictionary<String, Cryptocurrency> _nameToCryptocurrencyDictionary;

        /// <param name="cryptoStatsSource">CryptoStatsSource interface to be used</param>
        public CryptocurrencyResolverImpl(ICryptoStatsSource cryptoStatsSource)
        {
            _cryptoStatsSource = cryptoStatsSource;
        }

        public async Task Refresh()
        {
            // initialize the dictionary
            _nameToCryptocurrencyDictionary = new();
            
            // fetch all cryptocurrencies and add them to the dictionary using the symbol as a key
            (await _cryptoStatsSource.GetAvailableCryptocurrencies()).ForEach(c =>
                _nameToCryptocurrencyDictionary.TryAdd(c.Symbol, c));
        }

        public async Task<Cryptocurrency> Resolve(string symbol)
        {
            // refresh the dictionary if the symbol was not found in it
            if (_nameToCryptocurrencyDictionary?.GetValueOrDefault(symbol) == null) await Refresh();

            return _nameToCryptocurrencyDictionary.GetValueOrDefault(symbol, null);
        }
    }
}