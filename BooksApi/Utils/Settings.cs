using Microsoft.Extensions.Configuration;
using System.IO;

namespace BooksApi.Utils
{
    public static class Settings
    {
        public static string getMongoSetting(string settingName)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true).Build();

            return config.GetSection("BookstoreDatabaseSettings")[settingName];
        }
        public static string getRabbitSetting(string settingName)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true).Build();

            return config.GetSection("Rabbit")[settingName];
        }
    }
}
