
using Disruptor;
using PostTradeMonitor.Core.Interfaces;
using PostTradeMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostTradeMonitor.Core.Events
{
    public class PostTradeNotifier : IPostTradeNotifier
    {
        public event EventHandler<Trade> PostTradeTick = delegate { };

        public void NotifyOnTradePosted(Trade trade)
        {
            PostTradeTick(this, trade);
        }
    }

    public class PostTradeEventHandler : IEventHandler<Trade>
    {
        private IDisposable _subscription;
        private Dictionary<string, TradeSummary> tradeSummaryCache = new Dictionary<string, TradeSummary>();

        public Dictionary<string, TradeSummary> TradeSummary { get => tradeSummaryCache; private set => tradeSummaryCache = value; }
        public void OnEvent(Trade trade, long sequence, bool endOfBatch)
        {
            TradeSummary tradeSumary;


            if (tradeSummaryCache.TryGetValue(trade.Symbol, out tradeSumary))
            {
                tradeSumary.UpdateTradeSummary(trade);
            }
            else
            {
                tradeSummaryCache.Add(trade.Symbol, new TradeSummary(trade));
            }
        }
    }
}
