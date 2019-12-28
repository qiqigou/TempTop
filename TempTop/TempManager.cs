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

        public ITempBuild GetTempBuild()
        {
            ITempBuild build = default;
            using (var complier = new CSharpCodeProvider())
            {
                var dll = new CompilerParameters();
                dll.ReferencedAssemblies.Add("System.dll");
                dll.ReferencedAssemblies.Add("NewTonsoft.Json.dll");
                dll.ReferencedAssemblies.Add("TempTop.dll");
                dll.ReferencedAssemblies.Add("System.Core.dll");
                dll.GenerateExecutable = false;
                dll.GenerateInMemory = true;

                var cr = complier.CompileAssemblyFromSource(dll, GetCode());
                var error = cr.Errors.HasErrors;
                if (error == false)
                {
                    Assembly objAssembly = cr.CompiledAssembly;
                    build = objAssembly.CreateInstance("TempTop.CreateTemp") as ITempBuild;
                }
                return build;
            }
        }

        public void LoadFromFile(string path)
        {
            builder.Clear();
            using (var reader = new StreamReader(path))
            {
                builder.Append(reader.ReadToEnd());
            }
        }

        public void LoadFromString(string temp)
        {
            builder.Clear();
            builder.Append(temp);
        }

        private string GetCode()
        {
            var ana = new Analysis();
            return ana.Build(builder.ToString());
        }

    }
}
