namespace TempTop
{
    public class Temp: TempBase
    {
        protected override void Invoke()
        {
            Each(_data["usings"], (val, key) =>
             {
                 Output("using {0};", val);
             });
            Append();
            Output("namespace {0}", _data["namespace"]);
            Append("{");
            Append("	/// <summary>");
            Append("	/// 实体类");
            Append("	/// <summary>");
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
                Output("			this.{0} = new HashSet<{0}>();", val);
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
                Output("		public {0} {1} {{ get; set; }}", val["type"], val["name"]);
                Append();
            });
            Each(_data["forkey"], (val, key) =>
            {
                Output("		public virtual {0} {0} {{ get; set; }}", val);
                Append();
            });
            Each(_data["pkforkey"], (val, key) =>
            {
                Output("		public virtual ICollaction<{0}> {0} {{ get; set; }}", val);
                Append();
            });
            Append("	}");
            Append("}");
        }

    }
}
