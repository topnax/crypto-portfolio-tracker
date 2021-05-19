using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoStatsSource.model;

namespace CryptoStatsSource
{
    public interface ICryptoStatsSource
    {
        /// <summary>
        /// Gets all market entries matching the given IDs, price values are relative to the currency specified.
        /// </summary>
        /// <param name="currency">Code ("usd", "eur", "czk",...) of the currency the market entry prices should be relative to</param>
        /// <param name="ids">IDs of the market entries to be fetched</param>
        /// <returns>List of loaded market entries</returns>
        public Task<List<MarketEntry>> GetMarketEntries(string currency, params string[] ids);
        
        /// <summary>
        /// Gets a list of all available cryptocurrencies
        /// </summary>
        /// <returns>List of all available cryptocurrencies</returns>
        public Task<List<Cryptocurrency>> GetAvailableCryptocurrencies();
    }
}