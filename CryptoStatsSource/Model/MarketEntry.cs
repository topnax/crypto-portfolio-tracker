using Newtonsoft.Json;

namespace CryptoStatsSource.model
{
    public record MarketEntry(string Id, string Symbol, string Name, decimal CurrentPrice, long MarketCap,
        [JsonProperty("price_change_24h")] float PriceChange24H, [JsonProperty("price_change_percentage_24h")] float PriceChangePercentage24H);

    public record PriceEntry(string Id, string Symbol, string Name, decimal CurrentPrice,
        float PriceChangePercentage24H);

    public record Cryptocurrency(string Id, string Symbol, string Name);
}