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
        public ITempBuild GetTempBuild(string temp)
        {
            var baseUrl = Directory.GetCurrentDirectory();
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

                var cr = complier.CompileAssemblyFromSource(dll, GetCode(temp));
                var error = cr.Errors.HasErrors;
                if (error == false)
                {
                    Assembly objAssembly = cr.CompiledAssembly;
                    build = objAssembly.CreateInstance("TempTop.CreateTemp") as ITempBuild;
                }
                return build;
            }
        }

        private string GetCode(string temp)
        {
            var ana = new Analysis();
            return ana.Build(temp);
        }

    }
}
