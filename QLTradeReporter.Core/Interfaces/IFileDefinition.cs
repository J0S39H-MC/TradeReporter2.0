using QLTradeReporter.Core.Models;
using QLTradeReporter.Core.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTradeReporter.Core.Interfaces
{
    public interface IDelimitedFileDefinition
    {
        char Delimiter { get;  }
        FieldSchemaDictionary FieldSchemaDictionary { get; }
    }
}
