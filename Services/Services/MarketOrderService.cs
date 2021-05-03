using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Model;
using Repository;

namespace Services
{
    public interface IMarketOrderService
    {
        MarketOrder CreateMarketOrder(decimal filledPrice, decimal fee, decimal size,
            DateTime date, bool buy, int portfolioEntryId);

        bool DeleteMarketOrder(MarketOrder order);

        bool UpdateMarketOrder(MarketOrder order);

        MarketOrder GetMarketOrder(int id);

        List<MarketOrder> GetPortfolioEntryOrders(int portfolioEntryId);

        int DeletePortfolioEntryOrders(int portfolioEntryId);
    }

    public class MarketOrderServiceImpl : IMarketOrderService
    {
        private IMarketOrderRepository _marketOrderRepository;

        public MarketOrderServiceImpl(IMarketOrderRepository marketOrderRepository)
        {
            _marketOrderRepository = marketOrderRepository;
        }

        public MarketOrder CreateMarketOrder(decimal filledPrice, decimal fee, decimal size, DateTime date, bool buy,
            int portfolioEntryId)
        {
            var order = new MarketOrder(filledPrice, fee, size, date, buy, PortfolioEntryId: portfolioEntryId);
            var id = _marketOrderRepository.Add(order);
            return order with {Id = id};
        }

        public bool DeleteMarketOrder(MarketOrder order) => _marketOrderRepository.Delete(order);

        public bool UpdateMarketOrder(MarketOrder order) => _marketOrderRepository.Update(order);

        public MarketOrder GetMarketOrder(int id) => _marketOrderRepository.Get(id);

        public List<MarketOrder> GetPortfolioEntryOrders(int portfolioEntryId) =>
            _marketOrderRepository.GetAllByPortfolioEntryId(portfolioEntryId);

        public int DeletePortfolioEntryOrders(int portfolioEntryId) =>
            _marketOrderRepository.DeletePortfolioEntryOrders(portfolioEntryId);
    }
}