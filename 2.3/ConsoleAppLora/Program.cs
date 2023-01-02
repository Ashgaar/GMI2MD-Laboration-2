using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALoRa.Library;

namespace ConsoleApp.Lora
{
    public class Program
    {
        private static bool CONTAINER = false;
        static void Main(string[] args)
        {
            Console.WriteLine("\nALoRa ConsoleApp - A The Things Network C# Library\n");
            string TTN_APP_ID = "miol00-test-device@ttn";
            string TTN_ACCESS_KEY = "NNSXS.4UC7MI7DRMQOXTFJJOCO42M7UV4ZCUGVAVGO6ZY.O7LMGTORRNONL4EBPAT7OS7MWZ2WRDRZQTN5D6PQEBJEO6J6HUNQ";
            string TTN_REGION = "eu1";

            using (var app = new TTNApplication(TTN_APP_ID, TTN_ACCESS_KEY, TTN_REGION))
            {
                app.MessageReceived += App_MessageReceived;

                if (CONTAINER)
                {
                    // use for testing when running as container
                    Thread.Sleep(Timeout.Infinite);
                }
                else
                {
                    Console.WriteLine("Press return to exit!");
                    Console.ReadLine();
                    Console.WriteLine("\nAloha, Goodbye, Vaarwel!");
                    Thread.Sleep(1000);
                }
                app.Dispose();
            }
        }

        private static void App_MessageReceived(TTNMessage obj)
        {
            var data = obj.Payload != null ? BitConverter.ToString(obj.Payload) : string.Empty;
            Console.WriteLine($"Message Timestamp: {obj.Timestamp}, Device: {obj.DeviceID}, Topic: {obj.Topic}, Payload: {data}");

            var cleanText = ConvertToAscii(FromHex(data));


            Console.WriteLine($"Deciphered message: {cleanText}");
        }
        public static string ConvertToAscii(byte[] byteFormat)
        {
            var ascii = Encoding.ASCII.GetString(byteFormat);
            return ascii;
        }

        public static byte[] FromHex(string hex)
        {
            if (hex.Length == 0)
            {
                throw new ArgumentException(
                    "Input is empty");
            }
                    
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        /// <summary>
        /// Use this method for App.config files outside the app folder: https://stackoverflow.com/questions/10656077/what-is-wrong-with-my-app-config-file
        /// </summary>
        /// <param name="appSettingKey"></param>
        /// <returns>Appsetting value</returns>
        //public static string GetAppSettingValue(string appSettingKey)
        //{
        //    try
        //    {
        //        ExeConfigurationFileMap fileMap = new();
        //        fileMap.ExeConfigFilename = "/vm/conf/App.config";

        //        var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        //        var value = configuration.AppSettings.Settings[appSettingKey].Value;

        //        //var value = ConfigurationManager.AppSettings[appSettingKey];
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            var message = $"Cannot find value for appSetting key: '{appSettingKey}'.";
        //            throw new ConfigurationErrorsException(message);
        //        }
        //        return value;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"The appSettingKey: {appSettingKey} could not be read!");
        //        Console.WriteLine($"Exception: {e.Message}");
        //        return "";
        //    }
        //}


        ///// <summary>
        /////  /// Use this method for App.config files outside the app folder: https://stackoverflow.com/questions/10656077/what-is-wrong-with-my-app-config-file
        ///// </summary>
        ///// <param name="connectionStringKey"></param>
        ///// <returns>connectionString value</returns>
        //public static string GetConnectionStringValue(string connectionStringKey)
        //{
        //    try
        //    {
        //        ExeConfigurationFileMap fileMap = new();
        //        fileMap.ExeConfigFilename = "/vm/conf/App.config";

        //        var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        //        var value = configuration.ConnectionStrings.ConnectionStrings[connectionStringKey].ConnectionString;

        //        //var value = ConfigurationManager.ConnectionStrings[connectionStringKey].ToString();
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            var message = $"Cannot find value for connectionString key: '{connectionStringKey}'.";
        //            throw new ConfigurationErrorsException(message);
        //        }
        //        return value;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"The connectionStringKey: {connectionStringKey} could not be read!");
        //        Console.WriteLine($"Exception: {e.Message}");
        //        return "";
        //    }
        //}
    }
}
