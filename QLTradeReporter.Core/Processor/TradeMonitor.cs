using QLTradeReporter.Core.Interfaces;
using QLTradeReporter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace QLTradeReporter.Core.Processor
{
    public class TradeMonitor : IDisposable
    {
        private IDisposable _subscription;
        private Dictionary<string, TradeSummary> tradeSummaryCache = new Dictionary<string, TradeSummary>();

        public Dictionary<string, TradeSummary> TradeSummary { get => tradeSummaryCache; private set => tradeSummaryCache = value; }

        public TradeMonitor(IPostTradeNotifier ticker)
        {
            //creating an observable from the StockTick event, each notification will carry only the eventargs and will be synchronized
            IObservable<Trade> trades = Observable.FromEventPattern<EventHandler<Trade>, Trade>(h => ticker.PostTradeTick += h,
                                                                                                h => ticker.PostTradeTick -= h)
                                                                                                .Select(tickEvent => tickEvent.EventArgs)
                                                                                                .Synchronize();
            var groupedTrades = from trade in trades select trade ;
            _subscription = groupedTrades.Subscribe(trade =>
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
                                                             },
                                                            (e) => { }, //#C
                                                            () => { OnComplete(); }); 
        }

        private void OnComplete()
        {
             
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}
