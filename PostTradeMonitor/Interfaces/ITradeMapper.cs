using PostTradeMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostTradeMonitor.Core.Interfaces
{
    public interface ITradeMapper
    {
        T Map<T>(string fields) where T: Trade;
    }
}
