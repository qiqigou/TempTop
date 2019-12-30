using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    public class Users: ModelBase
    {
        //public Users() { }

        [Key]
        public string userid { get; set; }

        public string username { get; set; }

        public int age { get; set; }

        public string sex { get; set; }

    }

    public class ModelBase
    {

    }
}