using System;

namespace Model
{
    public record MarketOrder(Currency Currency, decimal FilledPrice, decimal Fee, decimal Size,
        DateTime Date, bool Buy, int Id = -1, int PortfolioEntryId = -1);

    public record Portfolio(string Name, string Description, int Id = -1);

    public record PortfolioEntry(string Symbol, int PortfolioId = -1, int Id = -1);

    public enum Currency
    {
        Czk,
        Eur,
        Usd
    }
}