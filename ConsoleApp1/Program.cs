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

            var build = new TempManager(tempPath).GetTempBuild();//编译模板（可复用）
            build.LoadFromFile(dataPath);//加载数据

            var code = build.Execute();//生成代码

            Console.WriteLine(code);
            Console.Read();
        }

    }
}
