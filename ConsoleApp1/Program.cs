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

        public static Assembly GetAssembly()
        {
            var baseUrl = Directory.GetCurrentDirectory();
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
            var prm = new Program();

            using (var db = new DALContext())
            {
                var list = prm.Filter(db, type);

                var json = JsonConvert.SerializeObject(list, Formatting.Indented);
                Console.WriteLine(json);
            }
            Console.Read();
        }


        public dynamic Filter(DALContext db, Type type)
        {
            var mk = typeof(Program).GetMethod("GetList").MakeGenericMethod(type);
            var list = mk.Invoke(this, new object[] { db });
            return list;
        }


        public List<T> GetList<T>(DALContext db) where T : class
        {
            var list = db.Set<T>().ToList();
            return list;
        }


    }
}
