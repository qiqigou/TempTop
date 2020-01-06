using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempTop
{
    public class TestTemp : TempBase
    {
        protected override void Invoke()
        {
            Each(_data["usings"], (val, key) =>
            {
                Output("using {0};", val);
            });
            Append("");
            Output("namespace {0}", _data["namespace"]);
            Append("{");
            Append("	/// <summary>");
            Append("	/// 实体类");
            Append("	/// </summary>");
            Append("	[DataContract]");
            Each(_data["forkey"], (val, key) =>
            {
                Output("	[KnownType(typeof({0}))]", val);
            });
            Output("	public class {0} : ModelBase", _data["class"]);
            Append("	{");
            Output("		public {0}()", _data["class"]);
            Append("		{");
            Each(_data["fields"], (val, key) =>
            {
                if (Compare(val["default"], "!=", ""))
                {
                    Output("			this.{0} = {1};", val["name"], val["default"]);
                }
            });
            Each(_data["pkforkey"], (val, key) =>
            {
                Output("			this.{0} = new HashSet<{1}>();", val, val);
            });
            Append("		}");
            Append();
            Each(_data["fields"], (val, key) =>
            {
                Append("		/// <summary>");
                Output("		/// {0}", val["desc"]);
                Append("		/// </summary>");
                Output("		[Description(\"{0}\")]", val["desc"]);
                Append("		[DataMenber]");
                if (Compare(val["key"], "==", 0))
                {
                    Append("		[Key]");
                }
                else if (Compare(val["key"], ">", 0))
                {
                    Output("		[Key, Column(Order = {0})]", val["key"]);
                }
                else
                {
                    Output("        {0}哈哈哈哈哈", key);
                }
                Output("		public {0} {1} {{ get; set; }}", val["type"], val["name"]);
                Append();
            });
            Each(_data["forkey"], (val, key) =>
            {
                Output("		public virtual {0} {1} {{ get; set; }}", val, val);
                Append();
            });
            Each(_data["pkforkey"], (val, key) =>
            {
                Output("		public virtual ICollaction<{0}> {1} {{ get; set; }}", val, val);
                Append();
            });
            Append("	}");
            Append("}");
        }
    }
}
