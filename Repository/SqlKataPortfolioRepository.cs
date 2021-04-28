using Database;
using Model;

namespace Repository
{
    public class SqlKataPortfolioRepository : SqlKataRepository<Portfolio>
    {
        public SqlKataPortfolioRepository(SqlKataDatabase db) : base(db, "portfolios")
        {
        }

        protected override int _getEntryId(Portfolio entry) => entry.Id;

        public override object ToRow(Portfolio entry) => new
        {
            name = entry.Name,
            description = entry.Description,
            currency_code = entry.CurrencyCode
        };

        public override Portfolio FromRow(dynamic d) =>
            new((string) d.name, (string) d.description, (int) d.currency_code, (int) d.id);
    }
}