using System;


namespace Model
{
    /// <summary>
    /// A record that represents a cryptocurrency portfolio
    /// </summary>
    /// <param name="Name">
    /// Name of the portfolio
    /// </param>
    /// <param name="Currency">
    /// Currency in which trades are made in the portfolio
    /// </param>
    /// <param name="Id">
    /// ID of the portfolio (defaults to -1)
    /// </param>
    public record Portfolio(string Name, string Description, Currency Currency, int Id = -1);

    /// <summary>
    /// A record that represents an entry of a portfolio 
    /// </summary>
    /// <param name="Symbol">
    /// A symbol of the cryptocurrency to be traded in within the entry
    /// </param>
    /// <param name="PortfolioId">
    /// ID of the portfolio this entry belongs to
    /// </param>
    /// <param name="Id">
    /// ID of the portfolio (defaults to -1)
    /// </param>
    public record PortfolioEntry(string Symbol, int PortfolioId = -1, int Id = -1);

    /// <summary>
    /// A record that represents a market order
    /// </summary>
    /// <param name="FilledPrice">
    /// Price of the asset the moment itwas traded
    /// </param>
    /// <param name="Fee">
    /// A fee for the trade made
    /// </param>
    /// <param name="Size">
    /// Order size
    /// </param>
    /// <param name="Date">
    /// Date when the trade was made
    /// </param>
    /// <param name="Buy">
    /// A flag indicating whether the trade was a buy or a sell
    /// </param>
    /// <param name="Id">
    /// ID of the order
    /// </param>
    /// <param name="PortfolioEntryId">
    /// ID of the portfolio entry this trade belongs to
    /// </param>
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

    /// <summary>
    /// An enumerable representing currencies
    /// </summary>
    public enum Currency : int
    {
        Czk = 203,
        Eur = 978,
        Usd = 849
    }
}