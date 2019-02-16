using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace QLTradeReporter.Core.Models
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
}
