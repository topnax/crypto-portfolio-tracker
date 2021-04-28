using System.Collections.Generic;
using Model;

namespace Repository
{
    // TODO comments
    public interface IRepository<T>
    {
        public object ToRow(T entry);

        public int Add(T entry);

        public T Get(int id);
        
        public List<T> GetAll();

        public bool Update(T entry);

        public bool Delete(T entry);
    }

    public interface IMarketOrderRepository : IRepository<MarketOrder>
    {
    }

    public interface IPortfolioRepository : IRepository<Portfolio>
    {
    }
}