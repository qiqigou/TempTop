using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempTop;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseUrl = Directory.GetCurrentDirectory();
            var temp = Path.Combine(@"E:\ASUS\Desktop\GitWork\TempTop\ConsoleApp1", "模板语法.cshtml");
            var data = Path.Combine(@"E:\ASUS\Desktop\GitWork\TempTop\ConsoleApp1", "模板数据.json");

            var manager = new TempManager();
            manager.LoadFromFile(temp);
            var build = manager.GetTempBuild();

            build.LoadFromFile(data);
            var str = build.Execute();

        }
    }
}
