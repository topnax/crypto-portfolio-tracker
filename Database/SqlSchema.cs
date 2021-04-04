namespace Database
{
    public class SqlSchema
    {
        public static void Init(SqlKataDatabase db)
        {
            db.Get().Statement(@"

                CREATE TABLE IF NOT EXISTS portfolios (
                   id           INTEGER NOT NULL PRIMARY KEY  AUTOINCREMENT,
                   name         TEXT NOT NULL,
                   description  TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS portfolio_entries (
                   id           INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                   symbol       TEXT NOT NULL,
                   portfolio_id INTEGER NOT NULL,
                   FOREIGN KEY(portfolio_id) REFERENCES portfolios(portfolio_id)
                );

                CREATE TABLE IF NOT EXISTS market_orders (
                   id           INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                   currency     TEXT NOT NULL,
                   filled_price INTEGER NOT NULL,
                   fee          INTEGER NOT NULL,
                   size         INTEGER NOT NULL,
                   date         INTEGER NOT NULL,
                   buy          INTEGER NOT NULL,
                   portfolio_entry_id INTEGER NOT NULL,
                   FOREIGN KEY(portfolio_entry_id) REFERENCES portfolio_entries(portfolio_entry_id)
                );

            ");
        }
    }
}