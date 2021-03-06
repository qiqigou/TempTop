﻿using Microsoft.CSharp;
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

        public ITempBuild GetTempBuild(params string[] pardlls)
        {
            var dlls = new List<string>
            {
                "System.dll",
                "System.Core.dll",
                "mscorlib.dll",
                "Microsoft.CSharp.dll",
            };
            if (pardlls == null || pardlls.Length == 0)
            {
                var nowdll = Assembly.GetExecutingAssembly().Location;
                var newtonsoft = Assembly.Load("Newtonsoft.Json").Location;
                dlls.Add(nowdll);
                dlls.Add(newtonsoft);
            }
            var ana = new Analysis();
            var code = ana.Build(builder.ToString());

            var assembly = CSharpHelper.GetAssembly(dlls.ToArray(), code);

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
