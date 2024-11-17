using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*---------- Get connection string & other info from appsetting.json Here.-------*/
/*--------- Pomeli 24.10.24---------- */

namespace Core
{

    public static class AppSettingHelper
    {
        private static IConfigurationRoot _configuration;
        static AppSettingHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
        }
        public static string GetSetting(string key, string getvalue)
        {
            return _configuration.GetSection(key)[getvalue];
        }
    }


}


