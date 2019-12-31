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
        public static Assembly Assembly;

        /// <summary>
        /// 取得Model程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetAssembly()
        {
            //var baseUrl = Directory.GetCurrentDirectory();
            var temp = Path.Combine(@"..\..\", "模板语法.cshtml");
            var data = Path.Combine(@"..\..\", "模板数据.json");

            var manager = new TempManager();
            manager.LoadFromFile(temp);
            var build = manager.GetTempBuild();

            build.LoadFromFile(data);
            var code = build.Execute();
            var dlls = new string[] {
                "System.ComponentModel.DataAnnotations.dll"
            };

            return CSharpHelper.GetAssembly(code, dlls);
        }

        static void Main(string[] args)
        {
            Assembly = GetAssembly();
            Type type = Assembly.GetType("ConsoleApp1.Users");

            var helper = new DBHelper();
            using (var db = new DALContext())
            {
                var list = helper.Filter(db, type, "userid=\"100\"");

                var json = JsonConvert.SerializeObject(list, Formatting.Indented);
                Console.WriteLine(json);
            }
            Console.Read();
        }




    }
}
