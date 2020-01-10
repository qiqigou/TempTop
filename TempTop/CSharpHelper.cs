using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace TempTop
{
    public class CSharpHelper
    {
        public static Assembly GetAssembly(string[] dlls, params string[] code)
        {
            Assembly assembly = null;
            using (var complier = new CSharpCodeProvider())
            {
                var dll = new CompilerParameters();
                dll.ReferencedAssemblies.AddRange(dlls);
                dll.GenerateExecutable = false;
                dll.GenerateInMemory = true;

                var cr = complier.CompileAssemblyFromSource(dll, code);
                var error = cr.Errors.HasErrors;
                if (error == false)
                {
                    assembly = cr.CompiledAssembly;
                }
                return assembly;
            }
        }

    }
}
