using System.Text.Json;
using Newtonsoft.Json;

namespace CryptoStatsSource.model
{
    public record MarketEntry(string Id, string Symbol, string Name, decimal CurrentPrice, long MarketCap,
        [JsonProperty("price_change_24h", NullValueHandling = NullValueHandling.Ignore)]
        float? PriceChange24H = 0f,
        [JsonProperty("price_change_percentage_24h", NullValueHandling = NullValueHandling.Ignore)]
        float? PriceChangePercentage24H = 0f
    );

    public record PriceEntry(string Id, string Symbol, string Name, decimal CurrentPrice,
        float PriceChangePercentage24H);

    public record Cryptocurrency(string Id, string Symbol, string Name);
}