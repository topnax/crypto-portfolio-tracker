using System;
using System.Collections.Generic;
using Model;
using Moq;
using Repository;
using Services;
using Xunit;

namespace Tests.Unit.Service
{
    public class MarketOrderServiceTest
    {
        [Fact]
        public void Create_CallsRepository()
        {
            // arrange
            var marketOrderToBeAdded = new MarketOrder(new decimal(12000.39), 12, new decimal(1.3),
                DateTime.Now.Subtract(TimeSpan.FromDays(30)), false,
                PortfolioEntryId: 1);

            var repositoryMock = new Mock<IMarketOrderRepository>();
            repositoryMock.Setup(x =>
                x.Add(It.Is<MarketOrder>(marketOrder => marketOrder == marketOrderToBeAdded))).Returns(1);
            var service = new MarketOrderServiceImpl(repositoryMock.Object);

            // act
            var marketOrder = service.CreateMarketOrder(new decimal(12000.39), 12, new decimal(1.3),
                DateTime.Now.Subtract(TimeSpan.FromDays(30)), false, 1);

            // assert
            Assert.Equal(marketOrderToBeAdded with {Id = 1}, marketOrder);
        }

        [Fact]
        public void Get_CallsRepository()
        {
            // arrange
            var marketOrderPresentInRepository = new MarketOrder(new decimal(12000.39), 12, new decimal(1.3),
                DateTime.Now.Subtract(TimeSpan.FromDays(30)), false,
                PortfolioEntryId: 1);
            var repositoryMock = new Mock<IMarketOrderRepository>();
            repositoryMock.Setup(x => x.Get(It.Is<int>(id => id == 1))).Returns(marketOrderPresentInRepository);
            var service = new MarketOrderServiceImpl(repositoryMock.Object);

            // act
            var marketOrder = service.GetMarketOrder(1);

            // assert
            Assert.Equal(marketOrderPresentInRepository, marketOrder);
        }

        [Fact]
        public void GetPortfolioEntries_CallsRepository()
        {
            // arrange
            var entriesList = new List<MarketOrder>
            {
                new(new Decimal(10000.39), 11, new Decimal(1.1), DateTime.Now, true,
                    PortfolioEntryId: 1),
                new(new Decimal(10000.39), 12, new Decimal(1.1), DateTime.Now, true,
                    PortfolioEntryId: 1),
                new(new Decimal(10000.39), 13, new Decimal(1.1), DateTime.Now, true,
                    PortfolioEntryId: 2),
                new(new Decimal(10000.11), 14, new Decimal(1.1), DateTime.Now, false,
                    PortfolioEntryId: 1),
            };

            var repositoryMock = new Mock<IMarketOrderRepository>();
            repositoryMock.Setup(x => x.GetAllByPortfolioEntryId(It.Is<int>(id => id == 1))).Returns(
                new List<MarketOrder>()
                {
                    entriesList[0], entriesList[1], entriesList[3]
                });
            var service = new MarketOrderServiceImpl(repositoryMock.Object);

            // act
            var entriesFetched = service.GetPortfolioEntryOrders(1);

            // assert
            Assert.Equal(new List<MarketOrder>
            {
                entriesList[0], entriesList[1], entriesList[3]
            }, entriesFetched);
        }

        [Fact]
        public void Update_CallsRepository()
        {
            // arrange
            var marketOrderToBeUpdated = new MarketOrder(new decimal(12000.39), 12, new decimal(1.3),
                DateTime.Now.Subtract(TimeSpan.FromDays(30)), false,
                PortfolioEntryId: 1);
            var repositoryMock = new Mock<IMarketOrderRepository>();
            repositoryMock.Setup(x => x.Update(It.IsAny<MarketOrder>())).Returns(true);
            var service = new MarketOrderServiceImpl(repositoryMock.Object);

            // act
            var updated = service.UpdateMarketOrder(marketOrderToBeUpdated);

            // assert
            Assert.True(updated);
        }

        [Fact]
        public void Delete_CallsRepository()
        {
            // arrange
            var marketOrderToBeDeleted = new MarketOrder(new decimal(12000.39), 12, new decimal(1.3),
                DateTime.Now.Subtract(TimeSpan.FromDays(30)), false,
                PortfolioEntryId: 1);
            var repositoryMock = new Mock<IMarketOrderRepository>();
            repositoryMock.Setup(x => x.Delete(It.IsAny<MarketOrder>())).Returns(true);
            var service = new MarketOrderServiceImpl(repositoryMock.Object);

            // act
            var delete = service.DeleteMarketOrder(marketOrderToBeDeleted);

            // assert
            Assert.True(delete);
        }

        [Fact]
        public void DeletePortfolioEntries_CallsRepository()
        {
            // arrange
            var portfolioEntryId = 15;
            var repositoryMock = new Mock<IMarketOrderRepository>();
            var service = new MarketOrderServiceImpl(repositoryMock.Object);

            // act
            service.DeletePortfolioEntryOrders(portfolioEntryId);

            // assert
            repositoryMock.Verify(x => x.DeletePortfolioEntryOrders(It.Is<int>(id => id == portfolioEntryId)));
        }
    }
}