
using QLTradeReporter.Core.Interfaces;
using QLTradeReporter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTradeReporter.Core.Events
{
    public class PostTradeNotifier : IPostTradeNotifier
    {
        public event EventHandler<Trade> PostTradeTick = delegate { };

        public void NotifyOnTradePosted(Trade trade)
        {
            PostTradeTick(this, trade);
        }
    }
}
