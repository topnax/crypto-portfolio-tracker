using System;


namespace Model
{
    public record Portfolio(string Name, string Description, Currency Currency, int Id = -1);

    public record PortfolioEntry(string Symbol, int PortfolioId = -1, int Id = -1);

    public record MarketOrder(decimal FilledPrice, decimal Fee, decimal Size,
        DateTime Date, bool Buy, int Id = -1, int PortfolioEntryId = -1)
    {
        public virtual bool Equals(MarketOrder? other)
        {
            // override the Equals operator so Dates are compared in a more intuitive way
            if (other == null) return false;

            return FilledPrice == other.FilledPrice && Fee == other.Fee && Size == other.Size &&
                   Date.ToString() == other.Date.ToString() && Buy == other.Buy && Id == other.Id &&
                   PortfolioEntryId == other.PortfolioEntryId;
        }
    }

    public enum Currency : int
    {
        Czk = 203,
        Eur = 978,
        Usd = 849
    }
}