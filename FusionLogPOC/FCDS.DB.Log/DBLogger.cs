using Fcds.Common.Models;
using Fcds.Common.Models.Interfaces;
using Fcds.log;
using Npgsql;
using System;

namespace Fcds.Log
{
    class DBLogger : IFcdsLogger
    {
        public void LogMessage(string message,string user="-",  LogMessageType logMessageType= LogMessageType.Trace, LogLevel logLevel = LogLevel.Info)
        {
            string connectionString = LogSettings.GetValue("LogDBConnectionString");
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "INSERT INTO public.\"Log\"(message, username, type, level, severity, datetime) VALUES (:message,:username,:type,:level,:severity,:datetimeParamName)";

                        cmd.Parameters.AddWithValue(":datetimeParamName", DateTime.Now);
                        cmd.Parameters.AddWithValue(":message", message);
                        cmd.Parameters.AddWithValue(":type", logMessageType.ToString());
                        cmd.Parameters.AddWithValue(":level", logLevel.ToString());

                        cmd.Parameters.AddWithValue(":severity", 0);
                        cmd.Parameters.AddWithValue(":username", user);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO
            }
        }

        public void LogMessage(string message, Exception exception, string user = "-", LogMessageType logMessageType= LogMessageType.Trace, LogLevel logLevel= LogLevel.Error)
        {
            message = message + " " + exception.Message + " " + exception.StackTrace;

            string connectionString = LogSettings.GetValue("LogDBConnectionString");
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "INSERT INTO public.\"Log\"(message, username, type, severity, datetime) VALUES (:message,:username,:type,:severity,:datetimeParamName)";

                        cmd.Parameters.AddWithValue(":datetimeParamName", DateTime.Now);
                        cmd.Parameters.AddWithValue(":message", message);
                        cmd.Parameters.AddWithValue(":type", logMessageType.ToString());
                        cmd.Parameters.AddWithValue(":severity", 0);
                        cmd.Parameters.AddWithValue(":username", user);
                        cmd.Parameters.AddWithValue(":level", logLevel);
                        cmd.ExecuteNonQuery();
                        if(exception.InnerException != null)
                        {
                            LogMessage("", exception.InnerException, user, logMessageType, logLevel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO
            }
        }
    }
}
