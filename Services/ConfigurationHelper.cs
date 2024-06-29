using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace E_Commerce_DataAccessLayer.Globals
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;
        
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public static string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }
       

        // Get ConnectionString

        //public static string ConnectionString()
        //{

        //    string path = "E:\\00-FullStack_DotNet_App\\02-E-Commerce_Application\\E-Commerce_Console_App\\bin\\Debug\\net6.0\\appConfig.json";
        //    var config = new ConfigurationBuilder()
        //        .AddJsonFile(path)
        //        .Build();

        //    string ConnectionStr = config.GetSection("ConnectionString").Value;

        //    return ConnectionStr;

        //}
    
    
    }
}
