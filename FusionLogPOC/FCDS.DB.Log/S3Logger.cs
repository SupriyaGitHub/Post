using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Fcds.Common.Models;
using Fcds.Common.Models.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fcds.Log
{
    class S3Logger : IFcdsLogger
    {
        private const string bucketName = "flogtest";
        private const string trace = "Trace";
        private const string pii = "Pii";
       
        private readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        public  IAmazonS3 client;

        private StringBuilder logTraceBuilder = new StringBuilder();
        private StringBuilder logPIIBuilder = new StringBuilder();


        public void LogMessage(string message, string user = "-", LogMessageType logMessageType = LogMessageType.Trace, LogLevel logLevel = LogLevel.Info)
        {
             
            if (logMessageType == LogMessageType.Trace)
            {               
                logTraceBuilder.AppendLine(string.Format(" {0} {1} {2} {3}", logLevel, user, DateTime.Now, message));
            }
            else
            {
                logPIIBuilder.AppendLine(string.Format(" {0} {1} {2} {3}", logLevel, user, DateTime.Now, message));
            }
        }

        public void LogMessage(string message, Exception exception, string user = "-", LogMessageType logMessageType = LogMessageType.Trace, LogLevel logLevel = LogLevel.Error)
        {
            message = message + " " + exception.Message + " " + exception.StackTrace;

            if (logMessageType == LogMessageType.Trace)
            {
                logTraceBuilder.AppendLine(string.Format(" {0} {1} {2} {3}", logLevel, user, DateTime.Now, message));
            }
            else
            {
                logPIIBuilder.AppendLine(string.Format(" {0} {1} {2} {3}", logLevel, user, DateTime.Now, message));
            }

            if (exception.InnerException != null)
            {
                LogMessage("", exception.InnerException, user, logMessageType, logLevel);
            }
        }

        public async Task WritingAnObjectAsync()
        {
            try
            {
                client = new AmazonS3Client(bucketRegion);
                // 1. Put object-specify only key name for the new object.
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = trace + DateTime.Now.ToShortTimeString().Replace('/', '-'),
                    ContentBody = logTraceBuilder.ToString()
                };

                var response = await client.PutObjectAsync(putRequest);

                var putRequest2 = new PutObjectRequest
                {
                    BucketName = "fcdspii",
                    Key = "pii" + DateTime.Now.ToShortTimeString().Replace('/', '-'),
                    ContentBody = logPIIBuilder.ToString()
                };

                var client2 = new AmazonS3Client(bucketRegion);
                var response2 = await client2.PutObjectAsync(putRequest2);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }
    }
}
