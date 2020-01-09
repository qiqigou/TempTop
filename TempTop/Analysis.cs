using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TempTop
{
    /// <summary>
    /// 将模板解析为C#代码
    /// </summary>
    internal class Analysis
    {
        protected readonly StringBuilder builder_result = new StringBuilder();
        protected readonly StringBuilder builder_fun1 = new StringBuilder();
        protected readonly StringBuilder builder_fun2 = new StringBuilder();
        protected readonly StringBuilder builder_fun3 = new StringBuilder();
        protected readonly StringBuilder builder_return = new StringBuilder();

        internal string Build(string temp)
        {
            builder_return.Clear();
            builder_return.Append("namespace TempTop");
            builder_return.Append("{");
            builder_return.Append("    public class CreateTemp : TempBuild");
            builder_return.Append("    {");
            builder_return.Append("        protected override void Invoke()");
            builder_return.Append("        {");
            builder_return.Append(Invoke(temp));
            builder_return.Append("        }");
            builder_return.Append("    }");
            builder_return.Append("}");
            return builder_return.ToString();
        }

        private string Invoke(string temp)
        {
            builder_fun3.Clear();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(temp));
            using (var reader = new StreamReader(stream))
            {
                do
                {
                    var result = string.Empty;
                    var input = reader.ReadLine();
                    if (input == null) break;
                    //声明变量
                    if (Regex.IsMatch(input, @"^\s*{{set\s+[\s\S]+}}\s*$"))
                    {
                        result = Set(input);
                    }
                    //each开始
                    else if (Regex.IsMatch(input, @"^\s*{{each\s+((?!{{|}}).)+}}\s*$"))
                    {
                        result = Each_start(input);
                    }
                    //each结束
                    else if (Regex.IsMatch(input, @"^\s*{{/each}}\s*$"))
                    {
                        result = "\n}));";
                    }
                    //if开始
                    else if (Regex.IsMatch(input, @"^\s*{{if\s+[\s\S]+}}\s*$"))
                    {
                        result = If_start(input);
                    }
                    //else if 开始
                    else if (Regex.IsMatch(input, @"^\s*{{else\s+if\s+[\s\S]+}}\s*$"))
                    {
                        result = ElseIf_start(input);
                    }
                    //else开始
                    else if (Regex.IsMatch(input, @"^\s*{{else}}\s*$"))
                    {
                        result = "\n}\nelse\n{";
                    }
                    //if结束
                    else if (Regex.IsMatch(input, @"^\s*{{/if}}\s*$"))
                    {
                        result = "\n}";
                    }
                    //表达式输出
                    else if (Regex.IsMatch(input, "({{[^{](((?!{{|}}).)+)}})+"))
                    {
                        result = Output(input);
                    }
                    //空行
                    else if (Regex.IsMatch(input, @"^\s+$"))
                    {
                        result = "\nAppend();";
                    }
                    //无表达式追加
                    else
                    {
                        result = Append(input);
                    }
                    builder_fun3.Append(result);
                } while (true);
            }
            return builder_fun3.ToString();
        }

        protected string Set(string input)
        {
            ClearResult();
            var code = Regex.Match(input, @"(?<={{set\s+)[\s\S]+(?=}})");
            builder_result.AppendFormat("\nvar {0};", code);
            return builder_result.ToString();
        }

        protected string Output(string input)
        {
            ClearResult();

            var index = 0;
            var startindex = 0;
            var mhs = Regex.Matches(input, @"{{[^{]((?!{{|}}).)+}}");

            foreach (Match item in mhs)
            {
                builder_fun1.Append(input.Substring(startindex, item.Index - startindex));
                builder_fun1.AppendFormat("{{{0}}}", index++);
                startindex = item.Index + item.Length;

                var vl = item.Value.Replace("{{", "").Replace("}}", "");
                builder_fun2.AppendFormat("{0} {1}", ",", vl);
            }
            builder_fun1.Append(input.Substring(startindex));
            builder_result.AppendFormat("\nOutput(\"{0}\"{1});", builder_fun1.Replace("\"", "\\\"").ToString(), builder_fun2.ToString());

            return builder_result.ToString();
        }

        protected string Each_start(string input)
        {
            ClearResult();
            var group = Regex.Match(input, @"(?<={{each\s+)(?<data>@?[\w.\[\]0-9]+)\s+(?<val>\w+)\s+(?<key>\w+)?").Groups;
            var data = group["data"].Value;
            var val = group["val"].Value;
            var key = group["key"].Value;

            var format = "\nEach({0}, (System.Action<dynamic, int>)(({1}, {2}) =>";
            if (string.IsNullOrWhiteSpace(key)) format = "\nEach({0}, (System.Action<dynamic, int>)(({1}) =>";

            builder_result.AppendFormat(format, data, val, key);
            builder_result.Append("\n{");
            return builder_result.ToString();
        }

        protected string If_start(string input)
        {
            ClearResult();
            builder_result.Append("\nif (");
            var code = Regex.Match(input, @"(?<={{if\s+)[\s\S]+(?=}})\s*").Value;
            builder_result.Append(code);
            builder_result.Append(")\n{");
            return builder_result.ToString();
        }

        protected string ElseIf_start(string input)
        {
            ClearResult();
            builder_result.Append("\n}\nelse if (");
            var code = Regex.Match(input, @"(?<={{else if\s+)[\s\S]+(?=}})\s*").Value;
            builder_result.Append(code);
            builder_result.Append(")\n{");
            return builder_result.ToString();
        }

        protected string Append(string input)
        {
            ClearResult();
            input = input.Replace("\"", "\\\"");
            builder_result.AppendFormat("\nAppend(\"{0}\");", input);
            return builder_result.ToString();
        }


        private void ClearResult()
        {
            builder_result.Clear();
            builder_fun1.Clear();
            builder_fun2.Clear();
        }

    }

}
