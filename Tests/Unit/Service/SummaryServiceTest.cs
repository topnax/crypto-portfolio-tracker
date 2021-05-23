using System;
using System.Collections.Generic;
using Model;
using Moq;
using Services;
using Xunit;

namespace Tests.Unit.Service
{
    public class SummaryServiceTest
    {
        [Fact]
        public void GetMarketOrderSummary_WithoutFee_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            MarketOrder order = new(10000m, 0m, 1m, DateTime.Now, true);
            var summary = service.GetMarketOrderSummary(order, 20000m);

            Assert.Equal(new ISummaryService.Summary(
                10000,
                1,
                20000,
                10000
            ), summary);
        }

        [Fact]
        public void GetMarketOrderSummary_WithFee_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            MarketOrder order = new(10000m, 1m, 1m, DateTime.Now, true);
            var summary = service.GetMarketOrderSummary(order, 20000m);

            Assert.Equal(new ISummaryService.Summary(
                9999,
                (20000m / (10000m + 1m)) - 1m,
                20000,
                10001
            ), summary);
        }

        [Fact]
        public void GetMarketOrderSummary_WithoutFee_InLoss_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            MarketOrder order = new(10000m, 0m, 1m, DateTime.Now, true);
            var summary = service.GetMarketOrderSummary(order, 5000m);

            Assert.Equal(new ISummaryService.Summary(
                -5000m,
                -0.5m,
                5000,
                10000
            ), summary);
        }


        [Fact]
        public void GetMarketOrderSummary_WithFee_InLoss_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            MarketOrder order = new(10000m, 1m, 1m, DateTime.Now, true);
            var summary = service.GetMarketOrderSummary(order, 5000m);

            Assert.Equal(new ISummaryService.Summary(
                -5001,
                (5000m / (10000m + 1m)) - 1m,
                5000,
                10001
            ), summary);
        }

        [Fact]
        public void GetMarketOrderSummary_ZeroCost_Returns_Zero_Summary()
        {
            var service = new SummaryServiceImpl();
            MarketOrder order = new(10000m, 0m, 0m, DateTime.Now, true);
            var summary = service.GetMarketOrderSummary(order, 5000m);

            Assert.Equal(new ISummaryService.Summary(
                0,
                0,
                0,
                0
            ), summary);
        }

        // PortfolioEntrySummary tests

        [Fact]
        public void GetPortfolioEntrySummary_InProfit_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            var summary = service.GetPortfolioEntrySummary(new()
            {
                new(10000m, 0m, 1m, DateTime.Now, true),
                new(20000m, 0m, 1m, DateTime.Now, true)
            }, 40000);

            Assert.Equal(new ISummaryService.Summary(
                30000m + 20000m,
                (80000m / 30000m) - 1m,
                80000m,
                30000
            ), summary);
        }

        [Fact]
        public void GetPortfolioEntrySummary_WithFee_InProfit_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            var summary = service.GetPortfolioEntrySummary(new()
            {
                new(10000m, 1m, 1m, DateTime.Now, true),
                new(20000m, 5m, 1m, DateTime.Now, true)
            }, 40000);
            Assert.Equal(new ISummaryService.Summary(
                30000m + 20000m - 6,
                ((80000m) / (30000m + 6m)) - 1m,
                80000m,
                30006
            ), summary);
        }

        [Fact]
        public void GetPortfolioEntrySummary_InProfit_WithSell_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            var summary = service.GetPortfolioEntrySummary(new()
            {
                new(10000m, 0m, 1m, DateTime.Now, true),
                new(20000m, 0m, 1m, DateTime.Now, true),
                new(30000m, 0m, 0.5m, DateTime.Now, false)
            }, 40000);

            Assert.Equal(new ISummaryService.Summary(
                1.5m * 40000m + 15000m - 30000m,
                ((1.5m * 40000m + 15000m - 30000m) / 30000m),
                1.5m * 40000m,
                30000
            ), summary);
        }

        [Fact]
        public void GetPortfolioEntrySummary_ZeroCost_Returns_Zero_Summary()
        {
            var service = new SummaryServiceImpl();
            var summary = service.GetPortfolioEntrySummary(new()
            {
                new(10000m, 0m, 0m, DateTime.Now, true),
            }, 40000);

            Assert.Equal(new ISummaryService.Summary(
                0m,
                0m,
                0m,
                0m
            ), summary);
        }

        [Fact]
        public void GetPortfolioEntrySummary_WithFee_InProfit_WithSell_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            var summary = service.GetPortfolioEntrySummary(new()
            {
                new(10000m, 1m, 1m, DateTime.Now, true),
                new(20000m, 5m, 1m, DateTime.Now, true)
            }, 40000);
            Assert.Equal(new ISummaryService.Summary(
                30000m + 20000m - 6,
                (80000m / (30000m + 6m)) - 1m,
                80000m,
                30006
            ), summary);
        }

        // Portfolio summary tests

        [Fact]
        public void GetPortfolioSummary_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            var summary = service.GetPortfolioSummary(new()
            {
                new(10000m, 1, 20000m, 10000),
                new(2000m, 2, 3000m, 1000),
            });
            Assert.Equal(
                new ISummaryService.Summary(12000m, (23000m / 11000m) - 1m, 23000m, 11000m),
                summary
            );
        }
        
            [Fact]
            public void GetPortfolioSummary_ZeroCost_Returns_ZeroSummary()
            {
                var service = new SummaryServiceImpl();
                var summary = service.GetPortfolioSummary(new()
                {
                    new(10000m, 1, 20000m, 0),
                    new(2000m, 2, 3000m, 0),
                });
                
                Assert.Equal(
                    new ISummaryService.Summary(0m, 0m, 0m, 0m),
                    summary
                );
            }
    }
}