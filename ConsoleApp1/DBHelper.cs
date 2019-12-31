using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class DBHelper
    {

        public dynamic Filter(DALContext db, Type type)
        {
            var mk = this.GetType().GetMethod("GetList").MakeGenericMethod(type);
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
