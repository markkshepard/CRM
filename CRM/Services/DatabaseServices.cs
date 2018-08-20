using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace CRM.Services
{
    public class DatabaseServices
    {
        public static string GetCRMDBConnectionString()
        {
            string connectionString = String.Empty;
            //default enviroinmnent is DEV
            string connectionToLoad = "CRMDev";
            // check to see if app is running in PROD envirnoment
            if (IsProdEnvironmentService())
            {
                // use Prod connection string instead
                connectionToLoad = "CRMProd";
            }
            connectionString = ConfigurationManager.ConnectionStrings[connectionToLoad].ConnectionString;
            return connectionString;
        }

        public static string GetEnvironmentService()
        {
            string env = String.Empty;
            // get environment user is running on
            env = HttpContext.Current.Request.Url.Host;
            return env;
        }

        private static bool IsProdEnvironmentService()
        {
            bool prodEnv = false;

            string env = GetEnvironmentService();

            if (env.Contains("bpgoals."))
            {
                prodEnv = true;
            }
            return prodEnv;
        }
    }
}