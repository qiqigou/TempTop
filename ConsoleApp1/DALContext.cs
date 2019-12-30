using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class DALContext : DbContext
    {
        public DALContext() : base("name=testdb1")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.AddFromAssembly(Program.Assembly);
            //Type type = Program.Assembly.GetType("ConsoleApp1.Users");
            //var obj = new EntityTypeConfiguration<Users>();

            var obj = GetType(Program.Type);
            modelBuilder.Configurations.Add(obj);
            base.OnModelCreating(modelBuilder);
        }



        public EntityTypeConfiguration<T> CreateEntity<T>() where T : class
        {
            return new EntityTypeConfiguration<T>();
        }

        private dynamic GetType(Type type)
        {
            MethodInfo mi = this.GetType().GetMethod("CreateEntity").MakeGenericMethod(type);
            dynamic obj = mi.Invoke(this, null);
            return obj;
        }
    }
}
