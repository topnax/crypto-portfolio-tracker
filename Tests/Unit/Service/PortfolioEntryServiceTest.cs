using System.Collections.Generic;
using Model;
using Moq;
using Repository;
using Services;
using Xunit;

namespace Tests.Unit.Service
{
    public class PortfolioEntryServiceTest
    {
        [Fact]
        public void Create_CallsRepository()
        {
            // arrange
            var portfolioEntryToBeAdded = new PortfolioEntry("btc", 1);
            var repositoryMock = new Mock<IPortfolioEntryRepository>();
            repositoryMock.Setup(x =>
                x.Add(It.Is<PortfolioEntry>(portfolioEntry => portfolioEntry == portfolioEntryToBeAdded))).Returns(1);
            var service = new PortfolioEntryServiceImpl(repositoryMock.Object);

            // act
            var portfolioEntry = service.CreatePortfolioEntry("btc", 1);

            // assert
            Assert.Equal(portfolioEntryToBeAdded with {Id = 1}, portfolioEntry);
        }

        [Fact]
        public void Get_CallsRepository()
        {
            // arrange
            var portfolioEntryPresentInRepository = new PortfolioEntry("btc", 1);
            var repositoryMock = new Mock<IPortfolioEntryRepository>();
            repositoryMock.Setup(x => x.Get(It.Is<int>(id => id == 1))).Returns(portfolioEntryPresentInRepository);
            var service = new PortfolioEntryServiceImpl(repositoryMock.Object);

            // act
            var portfolioEntry = service.GetPortfolioEntry(1);

            // assert
            Assert.Equal(portfolioEntryPresentInRepository, portfolioEntry);
        }

        [Fact]
        public void GetPortfolioEntries_CallsRepository()
        {
            // arrange
            var entriesList = new List<PortfolioEntry>
            {
                new("btc", 1, 1),
                new("ada", 2, 2),
                new("btc", 3, 3),
                new("ltc", 1, 4)
            };

            var repositoryMock = new Mock<IPortfolioEntryRepository>();
            repositoryMock.Setup(x => x.GetAllByPortfolioId(It.Is<int>(id => id == 1))).Returns(
                new List<PortfolioEntry>()
                {
                    entriesList[0], entriesList[3]
                });
            var service = new PortfolioEntryServiceImpl(repositoryMock.Object);

            // act
            var entriesFetched = service.GetPortfolioEntries(1);

            // assert
            Assert.Equal(new List<PortfolioEntry>
            {
                entriesList[0], entriesList[3]
            }, entriesFetched);
        }

        [Fact]
        public void Update_CallsRepository()
        {
            // arrange
            var entryToBeUpdated = new PortfolioEntry("btc", 1, 1);
            var repositoryMock = new Mock<IPortfolioEntryRepository>();
            repositoryMock.Setup(x => x.Update(It.IsAny<PortfolioEntry>())).Returns(true);
            var service = new PortfolioEntryServiceImpl(repositoryMock.Object);

            // act
            var updated = service.UpdatePortfolio(entryToBeUpdated);

            // assert
            Assert.True(updated);
        }

        [Fact]
        public void Delete_CallsRepository()
        {
            // arrange
            var entryToBeDeleted = new PortfolioEntry("btc", 1, 1);
            var repositoryMock = new Mock<IPortfolioEntryRepository>();
            repositoryMock.Setup(x => x.Delete(It.IsAny<PortfolioEntry>())).Returns(true);
            var service = new PortfolioEntryServiceImpl(repositoryMock.Object);

            // act
            var delete = service.DeletePortfolioEntry(entryToBeDeleted);

            // assert
            Assert.True(delete);
        }
    }
}