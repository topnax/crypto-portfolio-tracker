using System;
using Database;
using Model;

namespace Repository.MarketOrders
{
    public class SqlKataMarketOrderRepository : IMarketOrderRepository
    {
        private readonly SqlKataDatabase _db;

        public SqlKataMarketOrderRepository(SqlKataDatabase db)
        {
            _db = db;
        }

        public void AddMarketOrder(MarketOrder order)
        {
            throw new NotImplementedException();
        }

        public void UpdateMarketOrder(MarketOrder order)
        {
            throw new NotImplementedException();
        }

        public void DeleteMarketOrder(MarketOrder order)
        {
            throw new NotImplementedException();
        }
    }
}