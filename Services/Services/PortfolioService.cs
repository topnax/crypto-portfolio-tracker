using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Model;
using Repository;

namespace Services
{
    public interface IPortfolioService
    {
        Portfolio CreatePortfolio(string name, string description, Currency currency);
        bool DeletePortfolio(Portfolio portfolio);
        bool UpdatePortfolio(Portfolio portfolio);
        Portfolio GetPortfolio(int id);
        List<Portfolio> GetPortfolios();
    }

    public class PortfolioServiceImpl : IPortfolioService
    {
        private IPortfolioRepository _portfolioRepository;
        private IPortfolioEntryService _portfolioEntryService;

        public PortfolioServiceImpl(IPortfolioRepository portfolioRepository, IPortfolioEntryService portfolioEntryService)
        {
            this._portfolioRepository = portfolioRepository;
            this._portfolioEntryService = portfolioEntryService;
        }

        public Portfolio CreatePortfolio(string name, string description, Currency currency)
        {
            var potfolio = new Portfolio(name, description, currency);
            var id = _portfolioRepository.Add(potfolio);
            return potfolio with
            {
                Id = id
            };
        }

        public bool DeletePortfolio(Portfolio portfolio)
        {
            _portfolioEntryService.DeletePortfolioEntries(portfolio.Id);
            return _portfolioRepository.Delete(portfolio);
        }

        public bool UpdatePortfolio(Portfolio portfolio)
        {
            return _portfolioRepository.Update(portfolio);
        }

        public Portfolio GetPortfolio(int id)
        {
            return _portfolioRepository.Get(id);
        }

        public List<Portfolio> GetPortfolios()
        {
            return _portfolioRepository.GetAll();
        }
    }
}