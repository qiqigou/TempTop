namespace TempTop
{
    public class CreateTemp : TempBase
    {
        protected override void Invoke()
        {
            Each(_data.usings, (System.Action<dynamic, int>)((val, key) =>
            {
                Output("using {0};", val);
            }));
            Append("");
            Output("namespace {0}", _data.@namespace);
            Append("{");
            Output("	public class {0} : ModelBase", _data.@class);
            Append("	{");
            Each(_data.fields, (System.Action<dynamic, int>)((val, key) =>
            {
                if (val.key == 0)
                {
                    Append("		[Key]");
                }
                else if (val.key > 0)
                {
                    Output("		[Key, Column(Order = {0})]", val.key);
                }
                else
                {
                }
                Output("		public {0} {1} {{ get; set; }}", val.type, val.name);
                Append();
            }));
            Append("	}");
            Append("}");
        }
    }
}