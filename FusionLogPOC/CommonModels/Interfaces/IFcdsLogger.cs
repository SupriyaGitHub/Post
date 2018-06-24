using System;
using System.Collections.Generic;
using System.Text;

namespace Fcds.Common.Models.Interfaces
{
    public interface IFcdsLogger
    {
        void LogMessage(string message, string user = "-", LogMessageType logMessageType = LogMessageType.Trace, LogLevel logLevel = LogLevel.Info);
        void LogMessage(string message, Exception exception, string user = "-", LogMessageType logMessageType = LogMessageType.Trace, LogLevel logLevel = LogLevel.Error);
    }
}
