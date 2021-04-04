using System;
using Database;
using Model;

namespace Repository
{
    public class SqlKataMarketOrderRepository : SqlKataRepository<MarketOrder>
    {
        public SqlKataMarketOrderRepository(SqlKataDatabase db) : base(db, "market_orders")
        {
        }

        protected override int _getEntryId(MarketOrder entry) => entry.Id;

        public override object ToRow(MarketOrder entry) => new
        {
            currency = entry.Currency.ToString(),
            filled_price = (int) (entry.FilledPrice * 100),
            fee = (int) (entry.Fee * 100),
            size = (int) (entry.Size * 100),
            date = entry.Date.ToBinary(),
            buy = ((DateTimeOffset) entry.Date).ToUnixTimeSeconds(),
            portfolio_entrY_id = entry.PortfolioEntryId,
        };

        public override MarketOrder FromRow(dynamic d) => new(Currency.Czk, 0, 0, 0, DateTime.Now, true, 0, 0);
    }
}