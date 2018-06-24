using System;
using System.Collections.Generic;
using System.Text;

namespace Fcds.Common.Models
{
   public enum LogMessageType
    {
        PII =0,
        Trace=1
    }

    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning=2,
        Error=3
    }   
}
