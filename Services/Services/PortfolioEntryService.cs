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
        
        int DeletePortfolioEntries(int portfolioId);

        bool UpdatePortfolio(PortfolioEntry entry);

        PortfolioEntry GetPortfolioEntry(int id);

        List<PortfolioEntry> GetPortfolioEntries(int portfolioId);
    }

    public class PortfolioEntryServiceImpl : IPortfolioEntryService
    {
        private IPortfolioEntryRepository _portfolioEntryRepository;
        private IMarketOrderService _marketOrderService;

        public PortfolioEntryServiceImpl(IPortfolioEntryRepository portfolioEntryRepository, IMarketOrderService marketOrderService)
        {
            _portfolioEntryRepository = portfolioEntryRepository;
            _marketOrderService = marketOrderService;
        }


        public PortfolioEntry CreatePortfolioEntry(string symbol, int portfolioId)
        {
            var portfolioEntry = new PortfolioEntry(symbol, portfolioId);
            portfolioEntry = portfolioEntry with {Id = _portfolioEntryRepository.Add(portfolioEntry)};
            return portfolioEntry;
        }

        public bool DeletePortfolioEntry(PortfolioEntry entry)
        {
            _marketOrderService.DeletePortfolioEntryOrders(entry.Id);
            return _portfolioEntryRepository.Delete(entry);
        }

        public bool UpdatePortfolio(PortfolioEntry entry) => _portfolioEntryRepository.Update(entry);

        public PortfolioEntry GetPortfolioEntry(int id) => _portfolioEntryRepository.Get(id);

        public List<PortfolioEntry> GetPortfolioEntries(int portfolioId) => _portfolioEntryRepository.GetAllByPortfolioId(portfolioId);

        public int DeletePortfolioEntries(int portfolioId)
        {
            foreach (var portfolioEntry in GetPortfolioEntries(portfolioId))
            {
                _marketOrderService.DeletePortfolioEntryOrders(portfolioEntry.Id);
            }

            return _portfolioEntryRepository.DeletePortfolioEntries(portfolioId);
        }
    }
}