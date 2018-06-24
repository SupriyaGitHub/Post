using System;
using Fcds.Common.Models;
using Fcds.Common.Models.Interfaces;
using Fcds.log;

namespace Fcds.Log
{

    public class Logger : IFcdsLogger
    {
        S3Logger s3logger = new S3Logger();
        DBLogger logDB = new DBLogger();
        CloudWatchLogger cloudWatchLogger = new CloudWatchLogger();

        public Logger()
        {
            
        }

        public void WritingAnObjectAsync()
        {
            s3logger.WritingAnObjectAsync();
            cloudWatchLogger.WritingAnObjectAsync();
        }       

        public void LogMessage(string message, string user = "-", LogMessageType logMessageType = LogMessageType.Trace, LogLevel logLevel = LogLevel.Info)
        {
            foreach (var item in LogSettings.Get().Configuration)
            {
                switch (item.Name)
                {
                    case "LogToS3":
                        if (item.Value == "true")
                        {

                            s3logger.LogMessage( message,  user ,  logMessageType, logLevel );
                        }
                        break;
                    case "LogToCloudWatch":
                        if (item.Value == "true")
                        {

                            cloudWatchLogger.LogMessage(message, user, logMessageType, logLevel);
                        }
                        break;
                    case "LogToDatabase":
                        if (item.Value == "true")
                        {

                            logDB.LogMessage(message, user, logMessageType, logLevel);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void LogMessage(string message, Exception exception, string user = "-", LogMessageType logMessageType = LogMessageType.Trace, LogLevel logLevel = LogLevel.Error)
        {
            foreach (var item in LogSettings.Get().Configuration)
            {
                switch (item.Name)
                {
                    case "LogToS3":
                        if (item.Value == "true")
                        {

                            s3logger.LogMessage( message,  exception,  user , logMessageType ,  logLevel );
                        }
                        break;
                    case "LogToCloudWatch":
                        if (item.Value == "true")
                        {

                            cloudWatchLogger.LogMessage(message, exception, user, logMessageType, logLevel);
                        }
                        break;
                    case "LogToDatabase":
                        if (item.Value == "true")
                        {

                            logDB.LogMessage(message, exception, user, logMessageType, logLevel);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
