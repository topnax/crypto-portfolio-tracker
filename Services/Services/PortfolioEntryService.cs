using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Model;
using Repository;

namespace Services
{
    public interface IPortfolioEntryService
    {
        PortfolioEntry CreatePortfolioEntry(string symbol, int portfolioId);

        bool DeletePortfolioEntry(PortfolioEntry entry);

        bool UpdatePortfolio(PortfolioEntry entry);

        PortfolioEntry GetPortfolioEntry(int id);

        List<PortfolioEntry> GetPortfolioEntries(int portfolioId);
    }

    public class PortfolioEntryServiceImpl : IPortfolioEntryService
    {
        private IPortfolioEntryRepository _portfolioEntryRepository;

        public PortfolioEntryServiceImpl(IPortfolioEntryRepository portfolioEntryRepository)
        {
            _portfolioEntryRepository = portfolioEntryRepository;
        }


        public PortfolioEntry CreatePortfolioEntry(string symbol, int portfolioId)
        {
            var portfolioEntry = new PortfolioEntry(symbol, portfolioId);
            portfolioEntry = portfolioEntry with {Id = _portfolioEntryRepository.Add(portfolioEntry)};
            return portfolioEntry;
        }

        public bool DeletePortfolioEntry(PortfolioEntry entry)
        {
            return _portfolioEntryRepository.Delete(entry);
        }

        public bool UpdatePortfolio(PortfolioEntry entry)
        {
            return _portfolioEntryRepository.Update(entry);
        }

        public PortfolioEntry GetPortfolioEntry(int id)
        {
            return _portfolioEntryRepository.Get(id);
        }

        public List<PortfolioEntry> GetPortfolioEntries(int portfolioId)
        {
            return _portfolioEntryRepository.GetAllByPortfolioId(portfolioId);
        }
    }
}