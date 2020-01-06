using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TempTop;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var tempPath = Path.Combine(@"..\..\", "模板语法.txt");
            var dataPath = Path.Combine(@"..\..\", "模板数据.json");

            var build = new TempManager(tempPath).GetTempBuild();
            build.LoadFromFile(dataPath);
            var code = build.Execute();

            Console.WriteLine(code);
            Console.Read();
        }

    }
}
