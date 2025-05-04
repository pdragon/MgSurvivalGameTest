using System;
using System.IO;
using Newtonsoft.Json;
using Desktop.Classes;
using System.Text.Json;
using System.Reflection;
using static Desktop.Classes.ConfigFile;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;
using Desktop;


namespace Desktop
{
    public class AssetsDataLoader
    {
        public static ConfigFile Config { get; private set; }
        public ConfigFile GetFromJson() {
            if (Assembly.GetEntryAssembly() != null)
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                // Путь к файлу в папке Content
                //var json = File.ReadAllText(Path.Combine(path1, "cursorConfig.json"));
                var json = File.ReadAllText(Path.Combine(baseDirectory, "Content", "config.json"));
                var config = JsonConvert.DeserializeObject<ConfigFile>(json);
                Config = config;
                return config;
            }
            throw new System.Exception("Path to content not found");
        }

        //public void LoadAtlas(string Name)
        //{

        //}


    }
}
