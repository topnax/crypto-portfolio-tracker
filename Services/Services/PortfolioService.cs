using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Model;
using Repository;

namespace Services
{
    public interface IPortfolioService
    {
        Portfolio CreatePortfolio(string name, string description);
        bool DeletePortfolio(Portfolio portfolio);
        bool UpdatePortfolio(Portfolio portfolio);
        Portfolio GetPortfolio(int id);
        List<Portfolio> GetPortfolios();
    }

    public class PortfolioServiceImpl : IPortfolioService
    {
        private IPortfolioRepository _portfolioRepository;

        public PortfolioServiceImpl(IPortfolioRepository portfolioRepository)
        {
            this._portfolioRepository = portfolioRepository;
        }

        public Portfolio CreatePortfolio(string name, string description)
        {
            var id = _portfolioRepository.Add(new(name, description));
            return new(name, description, id);
        }

        public bool DeletePortfolio(Portfolio portfolio)
        {
            return _portfolioRepository.Delete(portfolio);
        }

        public bool UpdatePortfolio(Portfolio portfolio)
        {
            throw new NotImplementedException();
        }

        public Portfolio GetPortfolio(int id)
        {
            throw new NotImplementedException();
        }

        public List<Portfolio> GetPortfolios()
        {
            throw new NotImplementedException();
        }
    }
}