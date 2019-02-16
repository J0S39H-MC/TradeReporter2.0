using QLTradeReporter.Core.Interfaces;
using QLTradeReporter.Core.Models;
using QLTradeReporter.Core.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTradeReporter.Core.FileDefinitions
{
    /// <summary>
    /// Defines structure of the file.
    /// </summary>
    public class InputTradeFileDefinition : IDelimitedFileDefinition
    {
        public FieldSchemaDictionary FieldSchemaDictionary
        {
            // Defines the structure of the file independent of the processing/parser logic.
            // if the file structure changes i can update the structure here without touching any of the parser logic.
            // the file definition is tied to the poco model. 
            get
            {
                return new FieldSchemaDictionary()
                {
                    {0, new FieldSchema(0, nameof(Trade.TimeStamp), typeof(long)) },
                    {1, new FieldSchema(1, nameof(Trade.Symbol), typeof(string)){ RequiredLength = 3, AllowAlphaOnly = true } },
                    {2, new FieldSchema(2, nameof(Trade.Quantity), typeof(long)) },
                    {3, new FieldSchema(3, nameof(Trade.Price), typeof(int)) }
                };
            }
        }

        public char[] Delimiters { get => new char[] { ',' }; }
    }
}
