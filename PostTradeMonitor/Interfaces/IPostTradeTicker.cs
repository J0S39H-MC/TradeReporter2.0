using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostTradeMonitor.Core.Models;

namespace PostTradeMonitor.Core.Interfaces
{
    public interface IPostTradeNotifier
    {
        event EventHandler<Trade> PostTradeTick;

        void NotifyOnTradePosted(Trade trade);
    }
}
