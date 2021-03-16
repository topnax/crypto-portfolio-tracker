using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoStatsSource.model;
using Tiny.RestClient;

namespace CryptoStatsSource
{
    public class CoingeckoSource : ICryptoStatsSource
    {
        private const string BaseUrl = "https://api.coingecko.com/api/";
        private const string ApiVersion = "v3";

        private readonly TinyRestClient _client = new(new HttpClient(), $"{BaseUrl}{ApiVersion}/");

        public CoingeckoSource()
        {
            _client.Settings.Formatters.OfType<JsonFormatter>().First().UseSnakeCase();
        }

        public async Task<List<MarketEntry>> GetMarketEntries(string currency, params string[] ids) => await _client
                .GetRequest("coins/markets").AddQueryParameter("vs_currency", currency)
                .AddQueryParameter("ids", String.Join(",", ids)).ExecuteAsync<List<MarketEntry>>();
    }
}