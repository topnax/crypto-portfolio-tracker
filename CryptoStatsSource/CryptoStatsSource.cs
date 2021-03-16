using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoStatsSource.model;

namespace CryptoStatsSource
{
    public interface ICryptoStatsSource
    {
        public Task<List<MarketEntry>> GetMarketEntries(string currency, params string[] ids);
    }
}