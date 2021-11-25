using PostTradeMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostTradeMonitor.Core.Interfaces
{
    public interface ITradeParser
    {
        bool TryParseTrade(string line, out Trade trade);
        //bool TryParseTrade(byte[] line, out Trade trade);
    }
}
