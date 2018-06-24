using Fcds.Common.Models;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Fusion.API.Helper
{
    public class FusionRestClient
    {
        public List<LogSettingsModel> GetLogSettings()
        {

            List<LogSettingsModel> lstLogSettings = new List<LogSettingsModel>()
            {
                new LogSettingsModel(){ Name = "LogToS3", Value="true"},
                new LogSettingsModel(){ Name = "LogToCloudWatch", Value="true"},
                new LogSettingsModel(){ Name = "LogToDatabase", Value="true"},
                new LogSettingsModel(){ Name = "LogDBConnectionString", Value="host=fusioncds.cu1rwprqtnt5.us-east-1.rds.amazonaws.com; port = 5432; UserName = fusioncdsadmin; Password = fusioncdsadmin; Database = FCDSLog;"},
            };

            return lstLogSettings;

            //the settings are expected to come from database --> API .

            RestClient client = new RestClient("http://localhost:9075/api/");
            RestRequest request = new RestRequest("LogSettings", Method.GET);

            IRestResponse<List<LogSettingsModel>> response = client.Execute<List<LogSettingsModel>>(request);
            return response.Data;           
        }
    }
}
