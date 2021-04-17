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

        public override MarketOrder FromRow(dynamic d)
        {
            bool parsed = Enum.TryParse(d.currency, out Currency currency);
            if (!parsed)
            {
                throw new SqlKataRepositoryException($"Failed to parse currency {d.currency}");
            }

            return new(currency, d.filled_price, d.fee, d.size, Utils.Utils.UnixTimeStampToDateTime(d.date), d.buy > 0,
                0, 0);
        }
    }
}