using Fcds.Log;
using System;

namespace FusionLogPOC
{
    class Program
    {
        static void Main(string[] args)
        {

            
            Logger logger = new Logger();

            for (int i = 1000; i < 1010; i++)
            {
                logger.LogMessage(" This is a test message " + i.ToString());
            }
            for (int i = 2000; i < 2010; i++)
            {
                logger.LogMessage(" This is a test message " + i.ToString(),"test user" ,Fcds.Common.Models.LogMessageType.PII);
            }

            // efs
            // security
            // encryptions
            // partner specific 
            // encryption
            // db optimization
            // request end
            logger.WritingAnObjectAsync();
        }
    }
}
