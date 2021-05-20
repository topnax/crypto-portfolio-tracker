using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Model;
using Repository;

namespace Services
{
    /// <summary>
    /// A service that is responsible for managing portfolio entries and storing them to a persistent repository.
    /// </summary>
    public interface IPortfolioEntryService
    {
        /// <summary>
        /// Creates a new portfolio entry and adds it to a repository
        /// </summary>
        /// <param name="symbol">Cryptocurrency symbol</param>
        /// <param name="portfolioId">ID of the portfolio the entry should belong to</param>
        /// <returns>Created instance of the `PortfolioEntry` class</returns>
        PortfolioEntry CreatePortfolioEntry(string symbol, int portfolioId);

        /// <summary>
        /// Deletes the given portfolio entry from a repository
        /// </summary>
        /// <param name="entry">Entry to be deleted</param>
        /// <returns>A flag indicating whether the entry was deleted</returns>
        bool DeletePortfolioEntry(PortfolioEntry entry);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="portfolioId"></param>
        /// <returns></returns>
        int DeletePortfolioEntries(int portfolioId);

        /// <summary>
        /// Updates the given entry in the repository. The entry with the same ID in the repository is replaced with the one.
        /// </summary>
        /// <param name="entry">Updated entry to be stored in the repository</param>
        /// <returns>A flag indicating whether the portfolio entry was updated</returns>
        bool UpdatePortfolioEntry(PortfolioEntry entry);

        /// <summary>
        /// Loads and returns a portfolio entry from the database using the given ID.
        /// </summary>
        /// <param name="id">ID of the portfolio entry to be loaded</param>
        /// <returns>Found portfolio entry or `null`</returns>
        PortfolioEntry GetPortfolioEntry(int id);

        /// <summary>
        /// Returns all portfolio entries that belong to the portfolio identified by the given ID
        /// </summary>
        /// <param name="portfolioId">ID of the portfolio whose entries should be found</param>
        /// <returns>A list of all entries that belong to the given portfolio</returns>
        List<PortfolioEntry> GetPortfolioEntries(int portfolioId);
    }

    public class PortfolioEntryServiceImpl : IPortfolioEntryService
    {
        // dependency on the portfolio entry repository
        private readonly IPortfolioEntryRepository _portfolioEntryRepository;
        
        // dependency on the market order repository
        private readonly IMarketOrderService _marketOrderService;

        public PortfolioEntryServiceImpl(IPortfolioEntryRepository portfolioEntryRepository, IMarketOrderService marketOrderService)
        {
            _portfolioEntryRepository = portfolioEntryRepository;
            _marketOrderService = marketOrderService;
        }

        public PortfolioEntry CreatePortfolioEntry(string symbol, int portfolioId)
        {
            // create a new instance of the `PortfolioEntry` class
            var portfolioEntry = new PortfolioEntry(symbol, portfolioId);
            
            // add it to the repository and return it with the generated ID
            return portfolioEntry with {Id = _portfolioEntryRepository.Add(portfolioEntry)};
        }

        public bool DeletePortfolioEntry(PortfolioEntry entry)
        {
            // when deleting a portfolio entry make sure to delete all of its orders
            _marketOrderService.DeletePortfolioEntryOrders(entry.Id);
            
            // finally delete the portfolio entry
            return _portfolioEntryRepository.Delete(entry);
        }

        public bool UpdatePortfolioEntry(PortfolioEntry entry) => _portfolioEntryRepository.Update(entry);

        public PortfolioEntry GetPortfolioEntry(int id) => _portfolioEntryRepository.Get(id);

        public List<PortfolioEntry> GetPortfolioEntries(int portfolioId) => _portfolioEntryRepository.GetAllByPortfolioId(portfolioId);

        public int DeletePortfolioEntries(int portfolioId)
        {
            // iterate over all entries of the given portfolio
            foreach (var portfolioEntry in GetPortfolioEntries(portfolioId))
            {
                // delete all orders of each iterated portfolio entry
                _marketOrderService.DeletePortfolioEntryOrders(portfolioEntry.Id);
            }
            
            // finally delete entries of the portfolio
            return _portfolioEntryRepository.DeletePortfolioEntries(portfolioId);
        }
    }
}