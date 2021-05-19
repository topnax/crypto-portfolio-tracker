using System.Collections.Generic;
using Model;

namespace Repository
{
    /// <summary>
    /// An interface that represents a general repository for storing entities to a persistent storage
    /// </summary>
    /// <typeparam name="T">Type of the entity to be used</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Adds an entity to the repository
        /// </summary>
        /// <param name="entry">Entity to be added</param>
        /// <returns></returns>
        public int Add(T entry);

        /// <summary>
        /// Loads an entity specified by an ID from a repository
        /// </summary>
        /// <param name="id">ID of the entity to be loaded</param>
        /// <returns>Found entity or null</returns>
        public T Get(int id);
        
        /// <summary>
        /// Loads all entities from a repository
        /// </summary>
        /// <returns>Collection of all entities present in a repository</returns>
        public List<T> GetAll();

        /// <summary>
        /// Saves the updated version of an entity to the database overriding it's previous version
        /// </summary>
        /// <param name="entry">Updated version of already an entity already existing in the repository</param>
        /// <returns>A flag indicating whether the update was successful</returns>
        public bool Update(T entry);

        /// <summary>
        /// Deletes the given entity from the repository
        /// </summary>
        /// <param name="entry">An entity to be deleted from the repository</param>
        /// <returns>A flag indicating whether the entity was deleted successfully</returns>
        public bool Delete(T entry);
    }

    /// <summary>
    /// A repository interface used for storing market orders
    /// </summary>
    public interface IMarketOrderRepository : IRepository<MarketOrder>
    {
        /// <summary>
        /// Gets all market orders of a portfolio entry
        /// </summary>
        /// <param name="portfolioEntryId">ID of the entry whose orders should be loaded</param>
        /// <returns>A collection of all orders assigned to the portfolio entry</returns>
        public List<MarketOrder> GetAllByPortfolioEntryId(int portfolioEntryId);
        
        /// <summary>
        /// Deletes all market orders of the portfolio entry given by an ID
        /// </summary>
        /// <param name="portfolioEntryId">ID of the entry whose orders should be deleted</param>
        /// <returns>A flag indicating whether the orders have been successfully deleted</returns>
        public int DeletePortfolioEntryOrders(int portfolioEntryId);
    }

    /// <summary>
    /// A repository interface used for storing portfolios
    /// </summary>
    public interface IPortfolioRepository : IRepository<Portfolio>
    {
    }
    
    /// <summary>
    /// A repository interface used for storing portfolio entries
    /// </summary>
    public interface IPortfolioEntryRepository : IRepository<PortfolioEntry>
    {
        /// <summary>
        /// Gets all portfolio entries of a portfolio given by an ID
        /// </summary>
        /// <param name="portfolioId">ID of the portfolio whose entries should be loaded</param>
        /// <returns>A collection of all portfolio entries assigned to the portfolio</returns>
        public List<PortfolioEntry> GetAllByPortfolioId(int portfolioId);
        
        /// <summary>
        /// Deletes all portfolio entries of the portfolio given by an ID
        /// </summary>
        /// <param name="portfolioId">ID of the portfolio whose entries should be deleted</param>
        /// <returns>A flag indicating whether the entries have been succesfully deleted</returns>
        public int DeletePortfolioEntries(int portfolioId);
    }
}
