﻿using System;
using System.IO;
using Microsoft.Azure;
using Newtonsoft.Json.Linq;

namespace FieldWorkerBot.Bot.Infrastructure
{
    public class SettingsReader : ISettingsReader
    {
        public string this[string index]
        {
            get
            {
                var settingValue = CloudConfigurationManager.GetSetting(index);

                if (string.IsNullOrEmpty(settingValue))
                    settingValue = GetByLocalJsonFile(index);

                return settingValue;
            }
        }

        private string GetByLocalJsonFile(string index)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "FieldWorkerBotSettings.Json");
            if (!File.Exists(path))
                return string.Empty;

            var fileContent = File.ReadAllText(path);
            if (string.IsNullOrEmpty(fileContent))
                return string.Empty;

            var json = JObject.Parse(fileContent);

            if (!json.HasValues)
                return string.Empty;

            return json[index].Value<string>();
        }
    }
}