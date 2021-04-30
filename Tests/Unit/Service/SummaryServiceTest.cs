using System;
using System.Collections.Generic;
using Model;
using Moq;
using Services;
using Xunit;

namespace Tests.Unit.Service
{
    //public class SummaryServiceFixture : IDisposable
    //{
    //    public ISummaryService SummaryService;

    //    public SummaryServiceFixture()
    //    {
    //        var marketOrderServiceMock = new Mock<IMarketOrderService>();
    //        marketOrderServiceMock.Setup(x => x.GetPortfolioEntryOrders(It.IsAny<int>())).Returns(new List<MarketOrder>()
    //        {
    //           new (new decimal(10000), new decimal(2), 1, DateTime.Now, true)
    //        })
    //        
    //        var repositoryMock = new Mock<IPortfolioRepository>();
    //        repositoryMock.Setup(x => x.Add(It.Is<Portfolio>(portfolio => portfolio == portfolioToBeAdded))).Returns(1);
    //        SummaryService = new ()
    //    }
    //    
    //    public void Dispose()
    //    {
    //        
    //    }
    //}

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
        public void GetPortfolioEntrySummary_InProfit_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            MarketOrder order = new(10000m, 1m, 1m, DateTime.Now, true);
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
            MarketOrder order = new(10000m, 1m, 1m, DateTime.Now, true);
            var summary = service.GetPortfolioEntrySummary(new()
            {
                new(10000m, 1m, 1m, DateTime.Now, true),
                new(20000m, 5m, 1m, DateTime.Now, true)
            }, 40000);
// currentTotalHoldingValue + totalSellValue - totalCost - totalFee
            Assert.Equal(new ISummaryService.Summary(
                30000m + 20000m - 6,
                ((80000m - 6m) / 30000m) - 1m,
                80000m,
                30006
            ), summary);
        }

        [Fact]
        public void GetPortfolioEntrySummary_InProfit_WithSell_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            MarketOrder order = new(10000m, 1m, 1m, DateTime.Now, true);
            var summary = service.GetPortfolioEntrySummary(new()
            {
                new(10000m, 0m, 1m, DateTime.Now, true),
                new(20000m, 0m, 1m, DateTime.Now, true),
                new(30000m, 0m, 0.5m, DateTime.Now, false)
            }, 40000);

            Assert.Equal(new ISummaryService.Summary(
                1.5m * 40000m + 15000m - 30000m,
                ((1.5m * 40000m  + 15000m - 30000m) / 30000m),
                1.5m * 40000m,
                30000
            ), summary);
        }

        [Fact]
        public void GetPortfolioEntrySummary_WithFee_InProfit_WithSell_Returns_Correct_Summary()
        {
            var service = new SummaryServiceImpl();
            MarketOrder order = new(10000m, 1m, 1m, DateTime.Now, true);
            var summary = service.GetPortfolioEntrySummary(new()
            {
                new(10000m, 1m, 1m, DateTime.Now, true),
                new(20000m, 5m, 1m, DateTime.Now, true)
            }, 40000);
            // currentTotalHoldingValue + totalSellValue - totalCost - totalFee
            Assert.Equal(new ISummaryService.Summary(
                30000m + 20000m - 6,
                ((80000m - 6m) / 30000m) - 1m,
                80000m,
                30006
            ), summary);
        }
    }
}