using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using Model;

namespace Services
{
    public interface ISummaryService
    {
        public record Summary(decimal AbsoluteChange, decimal RelativeChange, decimal MarketValue, decimal Cost);

        public Summary GetMarketOrderSummary(MarketOrder order, decimal assetPrice);

        public Summary GetPortfolioEntrySummary(List<MarketOrder> portfolioEntryOrders, decimal assetPrice);

        public Summary GetPortfolioSummary(List<Summary> portfolioEntrySummaries);
    }

    public class SummaryServiceImpl : ISummaryService
    {
        public ISummaryService.Summary GetMarketOrderSummary(MarketOrder order, decimal assetPrice)
        {
            var marketValue = order.Size * assetPrice;
            var cost = (order.Size * order.FilledPrice) + order.Fee;
            if (cost == 0)
            {
                return new(0, 0, 0, 0);
            }

            var relativeChange = (marketValue / cost) - new decimal(1);
            var absoluteChange = marketValue - cost;
            return new(absoluteChange, relativeChange, marketValue, cost);
        }

        public ISummaryService.Summary GetAverageOfSummaries(IEnumerable<ISummaryService.Summary> summaries)
        {
            decimal totalMarketValue = 0;
            decimal totalCost = 0;
            decimal totalAbsoluteChange = 0;

            // iterate over all orders and compute their summaries
            foreach (var summary in summaries)
            {
                totalMarketValue += summary.MarketValue;
                totalCost += summary.Cost;
                totalAbsoluteChange += summary.AbsoluteChange;
            }

            if (totalCost == 0)
            {
                return new ISummaryService.Summary(0, 0, 0, 0);
            }

            decimal totalRelativeChange = (totalMarketValue / totalCost) - 1m;

            return new(totalAbsoluteChange, totalRelativeChange, totalMarketValue, totalCost);
        }

        public ISummaryService.Summary GetPortfolioEntrySummary(List<MarketOrder> portfolioEntryOrders,
            decimal assetPrice)
        {
            decimal totalHoldingSize = 0;
            decimal totalSellValue = 0;
            decimal totalCost = 0;
            decimal totalFee = 0;
            portfolioEntryOrders
                .ForEach(order =>
                {
                    totalHoldingSize += order.Size * (order.Buy ? 1 : -1);
                    var orderValue = order.Size * order.FilledPrice;
                    if (!order.Buy)
                    {
                        totalSellValue += orderValue;
                    }
                    else
                    {
                        totalCost += orderValue;
                    }

                    totalFee += order.Fee;
                });

            if (totalCost == 0)
            {
                return new ISummaryService.Summary(0, 0, 0, 0);
            }

            decimal currentTotalHoldingValue = totalHoldingSize * assetPrice;

            decimal totalAbsoluteChange = currentTotalHoldingValue + totalSellValue - totalCost - totalFee;
            decimal totalRelativeChange = totalAbsoluteChange / totalCost;

            return new ISummaryService.Summary(totalAbsoluteChange, totalRelativeChange, currentTotalHoldingValue,
                totalCost + totalFee);
        }

        public ISummaryService.Summary GetPortfolioSummary(List<ISummaryService.Summary> portfolioEntrySummaries) =>
            GetAverageOfSummaries(portfolioEntrySummaries);

    }
}