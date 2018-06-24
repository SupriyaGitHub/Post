using Fcds.Common.Models;
using Fcds.Common.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fcds.Log
{
    class CloudWatchLogger : IFcdsLogger
    {
        private StringBuilder logTraceBuilder = new StringBuilder();
        private StringBuilder logPIIBuilder = new StringBuilder();

        public void LogMessage(string message, string user = "-", LogMessageType logMessageType = LogMessageType.Trace, Common.Models.LogLevel logLevel = Common.Models.LogLevel.Info)
        {

            if (logMessageType == LogMessageType.Trace)
            {
                logTraceBuilder.AppendLine(string.Format(" {0} {1} {2} {3}", logLevel, user, DateTime.Now, message));
            }
            else
            {
                logPIIBuilder.AppendLine(string.Format(" {0} {1} {2}  {3}", logLevel, user, DateTime.Now, message));
            }
        }

        public void LogMessage(string message, Exception exception, string user = "-", LogMessageType logMessageType = LogMessageType.Trace, Common.Models.LogLevel logLevel = Common.Models.LogLevel.Error)
        {
            message = message + " " + exception.Message + " " + exception.StackTrace;

            if (logMessageType == LogMessageType.Trace)
            {
                logTraceBuilder.AppendLine(string.Format(" {0} {1} {2}  {3}", logLevel, user, DateTime.Now, message));
            }
            else
            {
                logPIIBuilder.AppendLine(string.Format(" {0} {1} {2}  {3}", logLevel, user, DateTime.Now, message));
            }

            if (exception.InnerException != null)
            {
                LogMessage("", exception.InnerException, user, logMessageType, logLevel);
            }
        }

       
        public async Task WritingAnObjectAsync()
        {
            var config = new AWS.Logger.AWSLoggerConfig("Fusion.AWSCloudWatch");
            config.Region = "us-east-1";
            config.LogGroup = "FcdsTrace";

            LoggerFactory logFactory = new LoggerFactory();

            logFactory.AddAWSProvider(config);
            var logger = logFactory.CreateLogger<CloudWatchLogger>();

            logger.LogInformation(logTraceBuilder.ToString());

            var configpii = new AWS.Logger.AWSLoggerConfig("Fusion.AWSCloudWatch");
            configpii.Region = "us-east-1";
            configpii.LogGroup = "Fcdspii";

            LoggerFactory logFactorypii = new LoggerFactory();

            logFactorypii.AddAWSProvider(configpii);
            var loggerpii = logFactorypii.CreateLogger<CloudWatchLogger>();

            loggerpii.LogInformation(logPIIBuilder.ToString());           
        }
    }
}
