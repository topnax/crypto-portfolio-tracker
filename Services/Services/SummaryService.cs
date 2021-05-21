using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using Model;

namespace Services
{
    /// <summary>
    /// A service that is responsible for calculating summaries (total profit, cost,...) of orders, portfolio entries and
    /// even portfolios. 
    /// </summary>
    public interface ISummaryService
    {
        /// <param name="AbsoluteChange">
        /// Absolute change of the of the tracked entity (typically in a USD, EUR,...) 
        /// </param>
        /// <param name="RelativeChange">
        /// Relative change of the of the tracked entity (0 meaning change whatsoever, 1.0 meaning 100% increase in market value,
        /// -1.0) meaning -100% decrease in market value 
        /// </param>
        /// <param name="MarketValue">
        /// Total market value of the tracked entity (entity size multiplied by the current entity value) 
        /// </param>
        /// <param name="Cost">
        /// Cost of the tracked entity (entity size multiplied by the price of the entity at the time it was traded)
        /// </param>
        public record Summary(decimal AbsoluteChange, decimal RelativeChange, decimal MarketValue, decimal Cost);

        /// <summary>
        /// Calculates a summary of the given market order
        /// </summary>
        /// <param name="order">Order whose summary should be calculated</param>
        /// <param name="assetPrice">Market price of the asset to be used when calculating the summary</param>
        /// <returns>Summary of the given order</returns>
        public Summary GetMarketOrderSummary(MarketOrder order, decimal assetPrice);

        /// <summary>
        /// Calculates a summary of the given list of orders of a portfolio entry
        /// </summary>
        /// <param name="portfolioEntryOrders">List of portfolio entries whose summary is to be calculated</param>
        /// <param name="assetPrice">Market price of the asset to be used when calculating the summary</param>
        /// <returns>A summary of the given list of orders</returns>
        public Summary GetPortfolioEntrySummary(List<MarketOrder> portfolioEntryOrders, decimal assetPrice);

        /// <summary>
        /// Calculates a summary of the given portfolio entry summaries  
        /// </summary>
        /// <param name="portfolioEntrySummaries">List of entry summaries whos summary is to be calculated</param>
        /// <returns>Summary of the given list of summaries</returns>
        public Summary GetPortfolioSummary(List<Summary> portfolioEntrySummaries);
    }

    public class SummaryServiceImpl : ISummaryService
    {
        public ISummaryService.Summary GetMarketOrderSummary(MarketOrder order, decimal assetPrice)
        {
            // order summary does not take into account whether it was a buy or a sell (same as Blockfolio app) 
            var marketValue = order.Size * assetPrice;
            var cost = (order.Size * order.FilledPrice) + order.Fee;
            if (cost == 0)
            {
                // when the cost is zero do not compute anything else
                return new(0, 0, 0, 0);
            }

            var relativeChange = (marketValue / cost) - new decimal(1);
            // absolute change is the difference between the current market value of the order subtracted by order's cost
            var absoluteChange = marketValue - cost;
            return new(absoluteChange, relativeChange, marketValue, cost);
        }

        public ISummaryService.Summary GetAverageOfSummaries(IEnumerable<ISummaryService.Summary> summaries)
        {
            decimal totalMarketValue = 0;
            decimal totalCost = 0;
            decimal totalAbsoluteChange = 0;

            // iterate over summaries and sum market value, cost and absolute change
            foreach (var summary in summaries)
            {
                totalMarketValue += summary.MarketValue;
                totalCost += summary.Cost;
                totalAbsoluteChange += summary.AbsoluteChange;
            }

            if (totalCost == 0)
            {
                // when the cost is zero, do not compute anything else
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
            
            // compute summary of market orders in the same was as the Blockfolio does
            portfolioEntryOrders
                .ForEach(order =>
                {
                    // determine the holding size (negative when the order was a sell) and add it tot he sum of all holdings
                    totalHoldingSize += order.Size * (order.Buy ? 1 : -1);
                    // compute the value of the order
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

            // current total holding value is computing by multiplying the total holding size with the given price of the asset
            decimal currentTotalHoldingValue = totalHoldingSize * assetPrice;

            // use the same formula as Blockfolio app does to compute the total absolute change
            decimal totalAbsoluteChange = currentTotalHoldingValue + totalSellValue - totalCost - totalFee;
            decimal totalRelativeChange = totalAbsoluteChange / (totalCost + totalFee);

            return new ISummaryService.Summary(totalAbsoluteChange, totalRelativeChange, currentTotalHoldingValue,
                totalCost + totalFee);
        }

        // summary of a portfolio is calculated by computing the average of all entry summaries present in it
        public ISummaryService.Summary GetPortfolioSummary(List<ISummaryService.Summary> portfolioEntrySummaries) =>
            GetAverageOfSummaries(portfolioEntrySummaries);
    }
}