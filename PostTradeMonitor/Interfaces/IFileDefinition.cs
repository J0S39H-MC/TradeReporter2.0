using PostTradeMonitor.Core.Models;
using PostTradeMonitor.Core.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostTradeMonitor.Core.Interfaces
{
    public interface IDelimitedFileDefinition
    {
        char[] Delimiters { get;  }
        FieldSchemaDictionary FieldSchemaDictionary { get; }
    }
}
