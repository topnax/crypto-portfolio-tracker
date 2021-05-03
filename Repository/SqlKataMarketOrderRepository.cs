using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Model;
using SqlKata.Execution;

namespace Repository
{
    public class SqlKataMarketOrderRepository : SqlKataRepository<MarketOrder>, IMarketOrderRepository
    {
        public SqlKataMarketOrderRepository(SqlKataDatabase db) : base(db, "market_orders")
        {
        }

        protected override int _getEntryId(MarketOrder entry) => entry.Id;

        public override object ToRow(MarketOrder entry)
        {
            return new
            {
                filled_price = (int) (entry.FilledPrice * 100),
                fee = (int) (entry.Fee * 100),
                size = (int) (entry.Size * 100),
                date = ((DateTimeOffset) entry.Date).ToUnixTimeSeconds(),
                buy = entry.Buy ? 1 : 0,
                portfolio_entry_id = entry.PortfolioEntryId,
            };
        }

        public override MarketOrder FromRow(dynamic d) =>
            new(Decimal.Divide(d.filled_price, 100), Decimal.Divide(d.fee, 100), Decimal.Divide(d.size, 100),
                DateTimeOffset.FromUnixTimeSeconds((int) d.date).DateTime.ToLocalTime(), d.buy > 0,
                (int) d.id, (int) d.portfolio_entry_id);

        public List<MarketOrder> GetAllByPortfolioEntryId(int portfolioEntryId) =>
            RowsToObjects(Db.Get().Query(tableName).Where("portfolio_entry_id", portfolioEntryId).Get());

        public int DeletePortfolioEntryOrders(int portfolioEntryOrder) =>
            Db.Get().Query(tableName).Where("portfolio_entry_id", portfolioEntryOrder).Delete();
    }
}