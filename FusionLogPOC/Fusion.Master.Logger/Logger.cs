using Amazon.S3;
using CommonModels;
using Fusion.AWS.S3;
using Fusion.AWSCloudWatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.Master.Logger
{

    public class Logger
    {
        
        public Logger()
        {
            
        }

        public void LogTraceMessage(string message)
        {
            foreach (var item in LogSettings.Get().Configuration)
            {
                switch (item.Name)
                {
                    case "LogToS3":
                        if(item.Value == "true")
                        {
                            S3Logger log = new S3Logger();
                            log.client = new AmazonS3Client(log.bucketRegion);
                            log.WritingAnObjectAsync(message).Wait();
                        }
                        break;
                    case "LogToCloudWatch":
                        if (item.Value == "true")
                        {
                            AWSCloudWatchLogger logc = new AWSCloudWatchLogger();
                            logc.log(message);
                        }
                        break;
                    case "LogToDatabase":
                        if (item.Value == "true")
                        {
                            FCDS.DB.Log.DBLogger logDB = new FCDS.DB.Log.DBLogger();
                            logDB.log(message);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void LogPIIMessage(string message)
        {

        }

    }
}
