using Microsoft.Extensions.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

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

        public static T JSONToObject<T>(string jsonString)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)serializer.ReadObject(ms);
                return obj;
            }
            catch
            {
                throw;
            }
        }
        public static string ObjectToJSON<T>(T obj)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, obj);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return jsonString;
            }
            catch
            {
                throw;
            }
        }

    }
}
