using Database;
using Model;

namespace Repository
{
    public class SqlKataPortfolioRepository : SqlKataRepository<Portfolio>, IPortfolioRepository
    {
        public SqlKataPortfolioRepository(SqlKataDatabase db) : base(db, SqlSchema.TablePortfolios)
        {
        }

        protected override int _getEntryId(Portfolio entry) => entry.Id;

        public override object ToRow(Portfolio entry) => new
        {
            name = entry.Name,
            description = entry.Description,
            currency_code = (int) entry.Currency
        };

        public override Portfolio FromRow(dynamic d) =>
            new((string) d.name, (string) d.description, (Currency) d.currency_code, (int) d.id);
    }
}