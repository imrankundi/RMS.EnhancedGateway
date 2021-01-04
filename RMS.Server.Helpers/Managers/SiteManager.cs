﻿using Newtonsoft.Json;
using RMS.Server.DataTypes;
using System;
using System.IO;

namespace RMS.Parser
{

    public class SiteManager
    {
        public Sites Sites { get; set; }
        public static SiteManager Instance { get; } = new SiteManager();
        private SiteManager()
        {
            ReadFromFile();
        }
        private string FolderPath()
        {
            return string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, "Sites");
        }
        private void ReadFromFile()
        {
            string folder = FolderPath();
            if (Directory.Exists(folder))
            {
                string filePath = string.Format(@"{0}\{1}", folder, "Sites.json");

                if (File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath);
                    Sites = JsonConvert.DeserializeObject<Sites>(text);
                }
            }
        }

        public void Reload()
        {
            ReadFromFile();
        }
    }
}