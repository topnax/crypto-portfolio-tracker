using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Model;
using Repository;

namespace Services
{
    /// <summary>
    /// A service that is responsible for managing portfolios and storing them to a persistent repository.
    /// </summary>
    public interface IPortfolioService
    {
        /// <summary>
        /// Creates a new portfolio and stores it to a repository
        /// </summary>
        /// <param name="name">Name of the portfolio</param>
        /// <param name="description">Description of the portfolio</param>
        /// <param name="currency">Currency to be used within the portfolio</param>
        /// <returns>A created instance of the `Portfolio` class</returns>
        Portfolio CreatePortfolio(string name, string description, Currency currency);

        /// <summary>
        /// Deletes the given portfolio from the repository 
        /// </summary>
        /// <param name="portfolio">Portfolio to be deleted from the repository</param>
        /// <returns>A flag indicating whether the portfolio was deleted</returns>
        bool DeletePortfolio(Portfolio portfolio);

        /// <summary>
        /// Updates the given portfolio in the repository. The portfolio with the same ID in the repository is replaced with the one.
        /// </summary>
        /// <param name="portfolio">Updated portfolio to be stored in the repository</param>
        /// <returns>A flag indicating whether the portfolio was updated</returns>
        bool UpdatePortfolio(Portfolio portfolio);

        /// <summary>
        /// Loads and returns a portfolio specified by the given ID from the repository.
        /// </summary>
        /// <param name="id">ID of the portfolio to be loaded from the repository</param>
        /// <returns>An instance of the `Portfolio` class that was loaded from the repository</returns>
        Portfolio GetPortfolio(int id);
        
        /// <summary>
        /// Returns a list of all portfolios present in the repository
        /// </summary>
        /// <returns>List of all portfolios present in the repository</returns>
        List<Portfolio> GetPortfolios();
    }

    public class PortfolioServiceImpl : IPortfolioService
    {
        // dependency on the portfolio repository
        private readonly IPortfolioRepository _portfolioRepository;
        
        // dependency on the portfolio entry service (in order to be able to delete entries when deleting a portfolio) 
        private readonly IPortfolioEntryService _portfolioEntryService;

        public PortfolioServiceImpl(IPortfolioRepository portfolioRepository,
            IPortfolioEntryService portfolioEntryService)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioEntryService = portfolioEntryService;
        }

        public Portfolio CreatePortfolio(string name, string description, Currency currency)
        {
            // create a new `Portfolio` class instance
            var portfolio = new Portfolio(name, description, currency);
            return portfolio with
            {
                Id = _portfolioRepository.Add(portfolio)
            };
        }

        public bool DeletePortfolio(Portfolio portfolio)
        {
            // first, delete all portfolio entries that belong to the portfolio being deleted
            _portfolioEntryService.DeletePortfolioEntries(portfolio.Id);
            
            // then finally delete the given portfolio
            return _portfolioRepository.Delete(portfolio);
        }

        public bool UpdatePortfolio(Portfolio portfolio) => _portfolioRepository.Update(portfolio);

        public Portfolio GetPortfolio(int id) => _portfolioRepository.Get(id);

        public List<Portfolio> GetPortfolios() => _portfolioRepository.GetAll();
        
    }
}