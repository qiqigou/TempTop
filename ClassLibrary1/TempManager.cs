using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ClassLibrary1
{
    public class TempManager
    {
        private StringBuilder builder = new StringBuilder();

        public TempManager() { }

        public TempManager(string path)
        {
            this.LoadFromFile(path);
        }

        public ITempBuild GetTempBuild()
        {
            var nowdll = Assembly.GetExecutingAssembly().Location;
            var newtonsoft = Assembly.Load("Newtonsoft.Json").Location;

            var dlls = new string[] {
                "System.dll",
                "System.Core.dll",
                "mscorlib.dll",
                "Microsoft.CSharp.dll",
                newtonsoft,
                nowdll,
            };
            var ana = new Analysis();
            var code = ana.Build(builder.ToString());

            var assembly = CSharpHelper.GetAssembly(dlls, code);

            return assembly.CreateInstance("TempTop.CreateTemp") as ITempBuild;
        }

        public void LoadFromFile(string path)
        {
            builder.Clear();
            using (var reader = new StreamReader(path, Encoding.UTF8))
            {
                builder.Append(reader.ReadToEnd());
            }
        }

        public void LoadFromString(string temp)
        {
            builder.Clear();
            builder.Append(temp);
        }

    }
}
