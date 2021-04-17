using Model;

namespace Repository
{
    public interface IRepository<T>
    {
        public object ToRow(T entry);

        public int Add(T entry);

        public T Get(int id);

        public void Update(T entry);

        public void Delete(T entry);
    }

    public interface IMarketOrderRepository : IRepository<MarketOrder>
    {
    }

    public interface IPortfolioRepository : IRepository<Portfolio>
    {
    }
}