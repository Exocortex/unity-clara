using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotnet_clara.lib
{
    public class Config
    {
        public ConfigInfo defaultConfig;
        public static string home = Application.persistentDataPath;
		public static string configFilePath = home + "/dotnetClara.json";

        public Config()
        {
            defaultConfig = new ConfigInfo
            {
                apiToken = "",
                basePath = "/api",
                host = "clara.io",
                username = ""
            };
        }

        public class ConfigInfo
        {
            public string apiToken { get; set; }
            public string basePath { get; set; }
            public string host { get; set; }
            public string username { get; set; }
        }


        
        // Write the default config to disk when starting.
        public void initializeConfig()
        {
                WriteConfig(defaultConfig);
        }

        // Write the config info to disk.
        public void WriteConfig(ConfigInfo configObj)
        {
            if (home == null)
            {
                Console.WriteLine("Invalid Home Directory!");
                return;
            }
            if(configFilePath == null)
            {
                Console.WriteLine("Invalid Configuration File Path!");
                return;
            }
            string output = JsonConvert.SerializeObject(configObj);
            if (output == null)
            {
                Console.WriteLine("Invalid Configuration!");
                return;
            }
			var sr = File.CreateText(configFilePath);
			sr.WriteLine(output);
			sr.Close();
        }

        // Read the config from disk.
        public ConfigInfo ReadConfig(string dir)
        {
            if (dir == null) dir = home;

			string configFilePath = dir + "/dotnetClara.json";

			System.IO.StreamReader jsonFile = new System.IO.StreamReader(configFilePath);
			string configFile = jsonFile.ReadToEnd();
			jsonFile.Close();

            if (configFile == null) return null;
            try
            {
                ConfigInfo configObj = JsonConvert.DeserializeObject<ConfigInfo>(configFile);
                return configObj;
            }
            catch (Exception e)
            {
                Console.Write("Invalid Configuration File:" + configFilePath);
                return null;
            }
        }

        // Get one config item value by key.
        public string GetOneConfigInfo(string key)
        {
            ConfigInfo configObj = ReadConfig(home);
            PropertyInfo[] properties = typeof(ConfigInfo).GetProperties();
            string value = null;
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == key)
                    value = (string)property.GetValue(configObj, null);
            }
            return value;
        }

        // Set the value of one config item.
        public void SetConfig(string key, string value)
        {
            ConfigInfo curConfig = ReadConfig(home);
            PropertyInfo[] properties = typeof(ConfigInfo).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == key)
                    property.SetValue(curConfig, value, null);
            }
            WriteConfig(curConfig);
        }
    }
}
