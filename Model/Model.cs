using System;

namespace Model
{
    public record MarketOrder(string Symbol, Currency Currency, decimal FilledPrice, decimal Fee, decimal Size,
        DateTime Date, bool Buy, int Id = -1, int PortfolioId = -1);

    public record Portfolio(string Name, string Description, int Id = -1);

    public enum Currency
    {
        Czk,
        Eur,
        Usd
    }
}