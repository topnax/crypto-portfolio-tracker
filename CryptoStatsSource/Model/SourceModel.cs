using Newtonsoft.Json;

namespace CryptoStatsSource.model
{
    /// <summary>
    /// A record representing a market entry (market evaluation of a given cryptocurrency)
    /// </summary>
    /// <param name="Id">
    /// Id of the market entry 
    /// </param>
    /// <param name="Symbol">
    /// Symbol of the market entry 
    /// </param>
    /// <param name="CurrentPrice">
    /// Price of the entry evaluated at the time of creation
    /// </param>
    /// <param name="MarketCap">
    /// Market cap of the entry
    /// </param>
    /// <param name="PriceChange24H">
    /// Price change of the entry in the last 24 hours
    /// </param>>
    /// <param name="PriceChangePercentage24H">
    /// Relative price change of the entry in the last 24 hours
    /// </param>
    public record MarketEntry(string Id, string Symbol, string Name, decimal CurrentPrice, long MarketCap,
        [JsonProperty("price_change_24h", NullValueHandling = NullValueHandling.Ignore)]
        float? PriceChange24H = 0f,
        [JsonProperty("price_change_percentage_24h", NullValueHandling = NullValueHandling.Ignore)]
        float? PriceChangePercentage24H = 0f
    );

    /// <summary>
    /// A record containing a basic information about a cryptocurrency
    /// </summary>
    /// <param name="Id">
    /// ID of the cryptocurrency
    /// </param>
    /// <param name="Symbol">
    /// Symbol of the cryptocurrency
    /// </param>
    /// <param name="Name">
    /// Name of the cryptocurrency
    /// </param>
    public record Cryptocurrency(string Id, string Symbol, string Name);
}