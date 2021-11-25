using PostTradeMonitor.Core.Extensions;
using PostTradeMonitor.Core.Interfaces;
using PostTradeMonitor.Core.Models;
using PostTradeMonitor.Extensions;
using System;
using System.Collections.Generic;

namespace PostTradeMonitor.Core.Processor
{
    public class TradeParser : ITradeParser
    {
        private readonly ITradeValidator validator;
        private readonly ITradeMapper tradeMapper;
        private readonly char[] delimiters;

        public TradeParser(ITradeValidator tradeValidator, ITradeMapper tradeMapper, IDelimitedFileDefinition fileDefinition)
        {
            this.validator = tradeValidator;
            this.tradeMapper = tradeMapper;
            this.delimiters = fileDefinition.Delimiters;
        }
        public bool TryParseTrade(string unparsedMessage, out Trade trade)
        {
            // remove exception throwing
            if (unparsedMessage == null || this.validator == null || tradeMapper == null || delimiters == null || delimiters.Length == 0) throw new ArgumentNullException();

            trade = null;

            if (unparsedMessage.Length == 0)
                return false;           

            trade = tradeMapper.Map<Trade>(unparsedMessage);

            return trade != null ? true : false;
        }
    }
}
