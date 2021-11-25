
using QLTradeReporter.Core.Interfaces;
using QLTradeReporter.Core.Models;
using System;

namespace QLTradeReporter.Core.Processor
{
    public class TradeParser : ITradeParser
    {
        private readonly ITradeValidator validator;
        private readonly ITradeMapper tradeMapper;
        private readonly char delimiters;

        public TradeParser(ITradeValidator tradeValidator, ITradeMapper tradeMapper, IDelimitedFileDefinition fileDefinition)
        {           
            this.validator = tradeValidator;
            this.tradeMapper = tradeMapper;
            this.delimiters = fileDefinition.Delimiter;
        }

        public bool TryParseTrade(string line, out Trade trade)
        {
            if (this.validator == null || tradeMapper == null || delimiters == null) throw new ArgumentNullException();

            string[] fields = line.Split(delimiters);

            trade = null;

            if (!validator.IsValid(fields))
            {
                return false;
            }

            trade = tradeMapper.Map<Trade>(fields);

            return trade != null ? true : false;
        }
    }
}
