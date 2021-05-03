using System.Collections.Generic;
using Model;
using Moq;
using Repository;
using Services;
using Xunit;

namespace Tests.Unit.Service
{
    public class PortfolioServiceTest
    {
        [Fact]
        public void Create_CallsRepository()
        {
            // arrange
            var portfolioToBeAdded = new Portfolio("Foo", "Bar", Currency.Eur, -1);
            var repositoryMock = new Mock<IPortfolioRepository>();
            var portfolioEntryServiceMock = new Mock<IPortfolioEntryService>();
            repositoryMock.Setup(x => x.Add(It.Is<Portfolio>(portfolio => portfolio == portfolioToBeAdded))).Returns(1);
            var service = new PortfolioServiceImpl(repositoryMock.Object, portfolioEntryServiceMock.Object);

            // act
            var portfolio = service.CreatePortfolio("Foo", "Bar", Currency.Eur);

            // assert
            Assert.Equal(portfolioToBeAdded with {Id = 1}, portfolio);
        }

        [Fact]
        public void Get_CallsRepository()
        {
            // arrange
            var portfolioToBeAdded = new Portfolio("Foo", "Bar", Currency.Eur, 1);
            var repositoryMock = new Mock<IPortfolioRepository>();
            repositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(portfolioToBeAdded);
            var portfolioEntryServiceMock = new Mock<IPortfolioEntryService>();
            var service = new PortfolioServiceImpl(repositoryMock.Object, portfolioEntryServiceMock.Object);

            // act
            var portfolio = service.GetPortfolio(1);

            // assert
            Assert.Equal(portfolioToBeAdded, portfolio);
        }

        [Fact]
        public void GetPortfolios_CallsRepository()
        {
            // arrange
            var portfolioList = new List<Portfolio>
            {
                new("My new portfolio", "Lorem ipsum dolor sit amet", Currency.Czk),
                new("My second portfolio", "Lorem ipsum dolor sit amet", Currency.Eur),
                new("My third portfolio", "Lorem ipsum dolor sit amet", Currency.Usd)
            };

            var repositoryMock = new Mock<IPortfolioRepository>();
            var portfolioEntryServiceMock = new Mock<IPortfolioEntryService>();
            repositoryMock.Setup(x => x.GetAll()).Returns(portfolioList);
            var service = new PortfolioServiceImpl(repositoryMock.Object, portfolioEntryServiceMock.Object);

            // act
            var portfolioListFetched = service.GetPortfolios();

            // assert
            Assert.Equal(portfolioList, portfolioListFetched);
        }

        [Fact]
        public void Update_CallsRepository()
        {
            // arrange
            var portfolioToBeUpdated = new Portfolio("Foo", "Bar", Currency.Eur, 1);
            var repositoryMock = new Mock<IPortfolioRepository>();
            var portfolioEntryServiceMock = new Mock<IPortfolioEntryService>();
            repositoryMock.Setup(x => x.Update(It.IsAny<Portfolio>())).Returns(true);
            var service = new PortfolioServiceImpl(repositoryMock.Object, portfolioEntryServiceMock.Object);

            // act
            var updated = service.UpdatePortfolio(portfolioToBeUpdated);

            // assert
            Assert.True(updated);
        }

        [Fact]
        public void Delete_CallsRepository_And_EntryService()
        {
            // arrange
            var portfolioToBeDeleted = new Portfolio("Foo", "Bar", Currency.Eur, 1);
            var repositoryMock = new Mock<IPortfolioRepository>();
            repositoryMock.Setup(x => x.Delete(It.IsAny<Portfolio>())).Returns(true);
            var portfolioEntryServiceMock = new Mock<IPortfolioEntryService>();
            var service = new PortfolioServiceImpl(repositoryMock.Object, portfolioEntryServiceMock.Object);

            // act
            var delete = service.DeletePortfolio(portfolioToBeDeleted);

            // assert
            portfolioEntryServiceMock.Verify(x => x.DeletePortfolioEntries(It.Is<int>(id => id == portfolioToBeDeleted.Id)));
            Assert.True(delete);
        }
    }
}