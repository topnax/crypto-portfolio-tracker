using System.Collections.Generic;
using Model;

namespace Repository
{
    // TODO comments
    public interface IRepository<T>
    {
        public int Add(T entry);

        public T Get(int id);
        
        public List<T> GetAll();

        public bool Update(T entry);

        public bool Delete(T entry);
    }

    public interface IMarketOrderRepository : IRepository<MarketOrder>
    {
        public List<MarketOrder> GetAllByPortfolioEntryId(int portfolioEntryId);
        public int DeletePortfolioEntryOrders(int portfolioEntryOrder);
    }

    public interface IPortfolioRepository : IRepository<Portfolio>
    {
    }
    
    public interface IPortfolioEntryRepository : IRepository<PortfolioEntry>
    {
        public List<PortfolioEntry> GetAllByPortfolioId(int portfolioId);
        
        public int DeletePortfolioEntries(int portfolioId);
    }
}
