using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
namespace PostTradeMonitor.Core.Models
{
    /// <summary>
    /// Represents a trade placed on an exchange
    /// </summary>
    public class Trade
    {
        public long TimeStamp { get; set; }

        public string Symbol { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }

    public  struct TradeInfo
    {
        public long TimeStamp { get; set; }

        public string Symbol { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }

    readonly public struct ImmutableTrade
    {
        public ImmutableTrade(long timeStamp, byte[] symbol, int qty, int price)
        {
            TimeStamp = timeStamp;
            Symbol = symbol;
            Quantity = qty;
            Price = price;
        }

        public long TimeStamp { get; }

        public byte[] Symbol { get;  }

        public int Quantity { get;  }

        public int Price { get;  }
    }
}
