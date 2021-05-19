using System.Collections.Generic;
using Database;
using Model;
using SqlKata.Execution;

namespace Repository
{
    public class SqlKataPortfolioEntryRepository : SqlKataRepository<PortfolioEntry>, IPortfolioEntryRepository
    {
        public SqlKataPortfolioEntryRepository(SqlKataDatabase db) : base(db, SqlSchema.TablePortfolioEntries)
        {
        }

        protected override int _getEntryId(PortfolioEntry entry) => entry.Id;

        public override object ToRow(PortfolioEntry entry) => new
        {
            symbol = entry.Symbol,
            portfolio_id = entry.PortfolioId
        };

        public override PortfolioEntry FromRow(dynamic d) => new((string) d.symbol, (int) d.portfolio_id, (int) d.id);

        public List<PortfolioEntry> GetAllByPortfolioId(int portfolioId) =>
            RowsToObjects(Db.Get().Query(tableName).Where(SqlSchema.PortfolioEntriesPortfolioId, portfolioId).Get());

        public int DeletePortfolioEntries(int portfolioId) =>
            Db.Get().Query(tableName).Where(SqlSchema.PortfolioEntriesPortfolioId, portfolioId).Delete();
    }
}