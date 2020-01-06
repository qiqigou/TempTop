using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TempTop
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
            var dlls = new string[] {
                "System.dll",
                "NewTonsoft.Json.dll",
                "TempTop.dll",
                "System.Core.dll",
                "mscorlib.dll",
                "Microsoft.CSharp.dll",
            };
            var ana = new Analysis();
            var code = ana.Build(builder.ToString());

            var assembly = CSharpHelper.GetAssembly(code, dlls);

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
