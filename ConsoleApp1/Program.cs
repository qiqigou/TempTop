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

            var path = Path.Combine(@"E:\ASUS\Desktop\GitWork\TempTop\ConsoleApp1", "模板语法.cshtml");
            var data = Path.Combine(@"E:\ASUS\Desktop\GitWork\TempTop\ConsoleApp1", "模板数据.json");
            using (var reader = new StreamReader(path))
            {
                var temp = reader.ReadToEnd();
                var manager = new TempManager();
                var build = manager.GetTempBuild(temp);
                build.LoadFromFile(data);
                var str = build.Execute();
                
            }

        }
    }
}
