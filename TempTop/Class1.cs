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
                dll.ReferencedAssemblies.Add("System.Data.dll");
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
            ana.tempPath = @"C:\Users\Administrator\source\repos\cs-scriptTest\cs-scriptTest\Temp\模板语法.cshtml";
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

        /// <summary>
        /// 从文件编译
        /// </summary>
        /// <param name="files">要编译的代码文件集合</param>
        /// <param name="referenceAssemblyNames">引用程序集名称集合</param>
        /// <param name="outputAssembly">输出dll名称</param>
        /// <returns>返回异常信息</returns>
        public static string CompileFromFile(string code, string[] referenceAssemblyNames, string outputAssembly)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters(referenceAssemblyNames, outputAssembly);
            //CompilerResults complierResult = codeProvider.CompileAssemblyFromFile(compilerParameters, files);
            CompilerResults complierResult = codeProvider.CompileAssemblyFromSource(compilerParameters, code);
            if (complierResult.Errors.HasErrors)
            {
                StringBuilder message = new StringBuilder();
                foreach (CompilerError err in complierResult.Errors)
                {
                    message.AppendFormat("(FileName:{0},ErrLine:{1}): error {2}: {3}", err.FileName, err.Line, err.ErrorNumber, err.ErrorText);
                }
                return message.ToString();
            }
            return string.Empty;
        }


        public void Test2()
        {
            string[] referenceAssemblyNames = new string[]
                {
                    "System.dll",
                    "NewTonsoft.Json.dll",
                    "TempTop.dll",
                    "System.Core.dll",
                    "System.Data.dll"
                };
            //输出dll
            string outputDll = @"C:\Users\Administrator\Desktop\code.dll";
            string errMsg = CompileFromFile(GetCode(), referenceAssemblyNames, outputDll);
        }

    }


}
