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
        // coingecko URL
        private const string BaseUrl = "https://api.coingecko.com/api/";
        
        // API version
        private const string ApiVersion = "v3";

        // initialise a HTTP client
        private readonly TinyRestClient _client = new(new HttpClient(), $"{BaseUrl}{ApiVersion}/");

        public CoingeckoSource()
        {
            // set JSON attribute name formatting to snake case
            _client.Settings.Formatters.OfType<JsonFormatter>().First().UseSnakeCase();
        }

        // make a call to the coins/markets API endpoint
        public async Task<List<MarketEntry>> GetMarketEntries(string currency, params string[] ids) => await _client
                .GetRequest("coins/markets").AddQueryParameter("vs_currency", currency)
                .AddQueryParameter("ids", String.Join(",", ids)).ExecuteAsync<List<MarketEntry>>();
        
        // make a call to the coins/list API endpoint
        public async Task<List<Cryptocurrency>> GetAvailableCryptocurrencies() => await _client
                .GetRequest("coins/list").ExecuteAsync<List<Cryptocurrency>>();
    }
}