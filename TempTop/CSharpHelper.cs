using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;

namespace TempTop
{
    public class CSharpHelper
    {
        public static Assembly GetAssembly(string[] dlls, params string[] code)
        {
            using (var complier = new CSharpCodeProvider())
            {
                var dll = new CompilerParameters();
                dll.ReferencedAssemblies.AddRange(dlls);
                dll.GenerateExecutable = false;
                dll.GenerateInMemory = true;

                var cr = complier.CompileAssemblyFromSource(dll, code);
                var error = cr.Errors.HasErrors;

                if (error)
                {
                    var sb = new StringBuilder();
                    foreach (var item in cr.Errors)
                    {
                        System.Console.WriteLine(item.GetType());
                    }
                    throw new System.Exception(sb.ToString());
                }

                return cr.CompiledAssembly;
            }
        }

    }
}
