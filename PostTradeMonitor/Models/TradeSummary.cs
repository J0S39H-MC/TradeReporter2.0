using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostTradeMonitor.Core.Models
{
    public class TradeSummary
    {
        private long volumePriceTotal;
        private long lastTime = 0;

        private TradeSummary()
        {
        }

        ////public TradeSummary(string symbol, string timeGap, int volumeTraded, int tradePrice)
        ////{
        ////}

        ////public TradeSummary(Trade trade, long timeGap)
        ////{
        ////    Symbol = trade.Symbol;
        ////    UpdateTradeSummary(trade, timeGap);
        ////}

        public TradeSummary(Trade trade)
        {
            Symbol = trade.Symbol;
            UpdateTradeSummary(trade);
        }

        public string Symbol { get; private set; }

        public long MaxTimeGap { get; private set; }

        public long Volume { get; private set; }

        public int MaxPrice { get; private set; }

        public long WeightedAveragePrice { get; private set; }


        public void UpdateTradeSummary(Trade trade)
        {
            // throw argument null exception?
            if (trade == null)
                return;

            UpdateMaxTime(trade);

            UpdateTotalVolume(trade);

            UpdateWAP(trade);

            UpdateMaxPrice(trade);
        }

        private void UpdateMaxPrice(Trade trade)
        {
            if (trade.Price > MaxPrice)
            {
                MaxPrice = trade.Price;
            }
        }

        private void UpdateWAP(Trade trade)
        {
            volumePriceTotal += (trade.Quantity * trade.Price);
            var weightedAveragePrice = volumePriceTotal / Volume;
            WeightedAveragePrice = weightedAveragePrice;
        }

        private void UpdateTotalVolume(Trade trade)
        {
            Volume += trade.Quantity;
        }

        private void UpdateMaxTime(Trade trade)
        {
            var gap = trade.TimeStamp - lastTime;
            if (gap == trade.TimeStamp)
            {
                lastTime = trade.TimeStamp;
            }
            else if (gap > MaxTimeGap)
            {
                MaxTimeGap = gap;
                lastTime = trade.TimeStamp;
            }

            //var gap = trade.TimeStamp - lastTime;
            //if (lastTime == 0)
            //{
            //    MaxTimeGap = 0;
            //}
            //if (gap > MaxTimeGap)
            //{
            //    MaxTimeGap = gap;
            //}
            //lastTime = trade.TimeStamp;
        }

        ////////public void UpdateTradeSummary(Trade trade, long timeGap)
        ////////{
        ////////    // throw argument null exception?
        ////////    if (trade == null)
        ////////        return;

        ////////    if (timeGap > MaxTimeGap)
        ////////    {
        ////////        MaxTimeGap = timeGap;
        ////////    }

        ////////    if (trade.Price > MaxTradePrice)
        ////////    {
        ////////        MaxTradePrice = trade.Price;
        ////////    }

        ////////    TotalVolumeTraded += trade.Quantity;
        ////////    volumePriceTotal += (trade.Quantity * trade.Price);
        ////////    var weightedAveragePrice = volumePriceTotal / TotalVolumeTraded;
        ////////    WAP = weightedAveragePrice;
        ////////}

    }
}
