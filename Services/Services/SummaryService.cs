using Model;

namespace Services
{
    public interface ISummaryService
    {
        public record Summary(decimal AbsoluteProfit, decimal RelativeProfit, decimal MarketValue);

        public Summary GetMarketOrderSummary(MarketOrder order, decimal assetPrice);
        
        public Summary GetPortfolioEntrySummary(PortfolioEntry entry, decimal assetPrice);
        
        public Summary GetPortfolioSummary(Portfolio entry, decimal assetPrice);
    }
    
    public class SummaryServiceImpl
    {
        
    }
}