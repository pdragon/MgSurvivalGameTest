using Shared;
using System;
using System.IO;

namespace Desktop.Core
{
    public class AppPath
    {
        public string Content {  get; set; }

        public AppPath()
        {
            string exePath = Environment.GetCommandLineArgs()[0];
            string exeDirectory = Path.GetDirectoryName(exePath);
            if (exeDirectory == null) {
                throw new ArgumentException("Error! Path of file not found");
            }
            //Content.RootDirectory
            //Content = Path.Combine(exeDirectory, "Content");
        }
    }
}
