using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace ConsoleApp1
{
    public class DBHelper
    {

        public dynamic Filter(DALContext db, Type type, string condition = "1=1")
        {
            var mk = this.GetType().GetMethod("GetList").MakeGenericMethod(type);
            var list = mk.Invoke(this, new object[] { db, condition });
            return list;
        }


        public List<T> GetList<T>(DALContext db, string condition) where T : class
        {
            var list = db.Set<T>().Where(condition).ToList();
            return list;
        }

    }
}
