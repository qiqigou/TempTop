using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TempTop
{
    public class Class1
    {
        public void Test()
        {
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
                var result = cr.Errors.HasErrors;
                if (result == false)
                {
                    Assembly objAssembly = cr.CompiledAssembly;
                    object objHelloWorld = objAssembly.CreateInstance("TempTop.TestTemp");
                    MethodInfo objMI = objHelloWorld.GetType().GetMethod("Execute");
                    objMI.Invoke(objHelloWorld, null);
                }
            }
        }

        public string GetCode()
        {
            var builder = new StringBuilder();
            var ana = new Analysis();
            ana.tempPath = @"C:\Users\Administrator\source\repos\TempTop\TempTop\Temp\模板语法.cshtml";
            var code = ana.Build();

            builder.Append("namespace TempTop");
            builder.Append("{");
            builder.Append("    public class TestTemp : TempTop.TempBase");
            builder.Append("    {");
            builder.Append("        protected override void Invoke()");
            builder.Append("        {");
            builder.Append(code);
            builder.Append("        }");
            builder.Append("    }");
            builder.Append("}");
            return builder.ToString();
        }

    }
}
