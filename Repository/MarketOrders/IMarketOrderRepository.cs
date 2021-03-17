using Model;

namespace Repository.MarketOrders
{
    public interface IMarketOrderRepository
    {
        public void AddMarketOrder(MarketOrder order);

        public void UpdateMarketOrder(MarketOrder order);

        public void DeleteMarketOrder(MarketOrder order);
    }
}