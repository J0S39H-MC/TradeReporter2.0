﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostTradeMonitor.Core.Interfaces
{
    public interface ITradeValidator
    {
        bool IsValid(string[] fields);
    }
}
