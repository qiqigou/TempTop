﻿namespace TempTop
{
    public class CreateTemp : TempBuild
    {
        protected override void Invoke()
        {
            Each(_data.usings, (System.Action<dynamic, int>)((val, key) =>
            {
                Output("using {0};", val);
            }));
            var str = _data.pkforkey[0];
            var nbr = 100 * 0.1;
            Output("namespace {0}", _data.@namespace);
            Append("{");
            Output("	public class {0} : ModelBase", _data.@class);
            Append("	{");
            Each(_data.fields, (System.Action<dynamic, int>)((val, key) =>
            {
                Output("		{0}{1}", nbr + key, str + val.desc);
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
                Output("		public {0} {1} { get; set; }", val.type, val.name);
                Append();
            }));
            if (_data.fields[0].name == "ap_ccode" && 1 == 1)
            {
                Output("		{0};", "wyl");
                Append("		\"������\"��");
            }
            Append("	}");
            Append("}");
        }
    }
}