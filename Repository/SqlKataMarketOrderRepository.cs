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
        private const int DecimalPrecision = 100000000;
        
        public SqlKataMarketOrderRepository(SqlKataDatabase db) : base(db, SqlSchema.TableMarketOrders)
        {
        }

        protected override int _getEntryId(MarketOrder entry) => entry.Id;

        public override object ToRow(MarketOrder entry)
        {
            return new
            {
                filled_price = (long) (entry.FilledPrice * DecimalPrecision),
                fee = (long) (entry.Fee * DecimalPrecision),
                size = (long) (entry.Size * DecimalPrecision),
                date = ((DateTimeOffset) entry.Date).ToUnixTimeSeconds(),
                buy = entry.Buy ? 1 : 0,
                portfolio_entry_id = entry.PortfolioEntryId,
            };
        }

        public override MarketOrder FromRow(dynamic d) =>
            new(Decimal.Divide(d.filled_price, DecimalPrecision), Decimal.Divide(d.fee, DecimalPrecision), Decimal.Divide(d.size, DecimalPrecision),
                DateTimeOffset.FromUnixTimeSeconds((int) d.date).DateTime.ToLocalTime(), d.buy > 0,
                (int) d.id, (int) d.portfolio_entry_id);

        public List<MarketOrder> GetAllByPortfolioEntryId(int portfolioEntryId) =>
            RowsToObjects(Db.Get().Query(tableName).Where(SqlSchema.MarketOrdersPortfolioEntryId, portfolioEntryId).Get());

        public int DeletePortfolioEntryOrders(int portfolioEntryOrder) =>
            Db.Get().Query(tableName).Where(SqlSchema.MarketOrdersPortfolioEntryId, portfolioEntryOrder).Delete();
    }
}