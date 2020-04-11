using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ClassLibrary1
{
    public class CSharpHelper
    {
        public static Assembly GetAssembly(string[] dlls, params string[] code)
        {
            var apiRemoteAsmName = "demo";
            var references = new List<MetadataReference>();

            foreach (var item in dlls)
            {
                //var path = Assembly.Load(item).Location;
                references.Add(MetadataReference.CreateFromFile(item));
            }

            var tree = SyntaxFactory.ParseSyntaxTree(code[0]);

            var compilation = CSharpCompilation.Create(apiRemoteAsmName)
                                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                                .AddReferences(references)
                                .AddSyntaxTrees(tree);
            var stream = new MemoryStream();
            var com = compilation.Emit(stream);
            var bins = new BinaryReader(stream);

            return Assembly.Load(bins.ReadBytes((int)stream.Length));

            //执行编译
            //var compilationResult = compilation.Emit(apiRemoteProxyDllFile);
            //if (compilationResult.Success)
            //{
            //    result = Assembly.LoadFrom(apiRemoteProxyDllFile);
            //}
            //else
            //{
            //    foreach (Diagnostic codeIssue in compilationResult.Diagnostics)
            //    {
            //        string issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}," +
            //            $" Location: { codeIssue.Location.GetLineSpan()}, " +
            //            $"Severity: { codeIssue.Severity}";
            //        Console.WriteLine("自动编译代码出现异常," + issue);
            //    }
            //}
            //return result;
        }

    }
}
