using System;

namespace Database
{
    public class SqlSchema
    {
        public const string TableIdPrimaryKey = "id";
        
        public const string TablePortfolios = "portfolios";
        public const string PortfoliosId = TableIdPrimaryKey;
        public const string PortfoliosName = "name";
        public const string PortfoliosDescription = "description";
        public const string PortfoliosCurrencyCode = "currency_code";
        
        public const string TablePortfolioEntries = "portfolio_entries";
        public const string PortfolioEntriesId = TableIdPrimaryKey;
        public const string PortfolioEntriesSymbol = "symbol";
        public const string PortfolioEntriesPortfolioId = "portfolio_id";
        
        public const string TableMarketOrders = "market_orders";
        public const string MarketOrdersId = TableIdPrimaryKey;
        public const string MarketOrdersFilledPrice = "filled_price";
        public const string MarketOrdersFee = "fee";
        public const string MarketOrdersSize = "size";
        public const string MarketOrdersDate = "date";
        public const string MarketOrdersBuy = "buy";
        public const string MarketOrdersPortfolioEntryId = "portfolio_entry_id";
        
        public static void Init(SqlKataDatabase db)
        {
            db.Get().Statement($@"

                CREATE TABLE IF NOT EXISTS {TablePortfolios} (
                   {PortfoliosId}            INTEGER NOT NULL PRIMARY KEY  AUTOINCREMENT,
                   {PortfoliosName}          TEXT NOT NULL,
                   {PortfoliosDescription}   TEXT NOT NULL,
                   {PortfoliosCurrencyCode} INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS {TablePortfolioEntries} (
                   {PortfolioEntriesId}                           INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                   {PortfolioEntriesSymbol}                       TEXT NOT NULL,
                   {PortfolioEntriesPortfolioId}                 INTEGER NOT NULL,
                   FOREIGN KEY({PortfolioEntriesPortfolioId})    REFERENCES {TablePortfolios}(id)
                );

                CREATE TABLE IF NOT EXISTS {TableMarketOrders} (
                   {MarketOrdersId}           INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                   {MarketOrdersFilledPrice} INTEGER NOT NULL,
                   {MarketOrdersFee}          INTEGER NOT NULL,
                   {MarketOrdersSize}         INTEGER NOT NULL,
                   {MarketOrdersDate}         INTEGER NOT NULL,
                   {MarketOrdersBuy}          INTEGER NOT NULL,
                   {MarketOrdersPortfolioEntryId} INTEGER NOT NULL,
                   FOREIGN KEY({MarketOrdersPortfolioEntryId}) REFERENCES {TablePortfolioEntries}(id)
                );

            ");
        }
    }
}