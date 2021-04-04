using Database;
using Model;

namespace Repository
{
    public class SqlKataPortfolioEntryRepository : SqlKataRepository<PortfolioEntry>
    {
        public SqlKataPortfolioEntryRepository(SqlKataDatabase db) : base(db, "portfolio_entries")
        {
        }

        protected override int _getEntryId(PortfolioEntry entry) => entry.Id;

        public override object ToRow(PortfolioEntry entry) => new
        {
            symbol = entry.Symbol,
            portfolio_id = entry.PortfolioId
        };
        
        public override PortfolioEntry FromRow(dynamic d) => new(d.symbol, d.portfolio_entry_id, d.id);
    }
}