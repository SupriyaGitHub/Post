using Fcds.Common.Models;
using Fusion.API.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Fcds.log
{
    public class LogSettings
    {
        private static LogSettings objLogSettingsSingleton;
        public List<LogSettingsModel> Configuration;


        private LogSettings()
        {
            Configuration = (new FusionRestClient()).GetLogSettings();
        }

        public static LogSettings Get()
        {
            if(objLogSettingsSingleton == null)
            {
                objLogSettingsSingleton = new LogSettings();               

            }
            return objLogSettingsSingleton;
        }

        public static string GetValue(string key)
        {
            if (objLogSettingsSingleton == null)
            {
                objLogSettingsSingleton = new LogSettings();

            }
            if(objLogSettingsSingleton.Configuration.FirstOrDefault(e=>e.Name == key) != null)
            {
                return objLogSettingsSingleton.Configuration.First(e => e.Name == key).Value;
            }
            return string.Empty;
        }
    }
}
