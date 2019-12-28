using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TempTop
{
    public class Analysis
    {
        public string tempPath { get; set; }
        protected StringBuilder exprBuilder { get; } = new StringBuilder();
        protected StringBuilder builder_result { get; } = new StringBuilder();
        protected StringBuilder builder_fun1 { get; } = new StringBuilder();
        protected StringBuilder builder_fun2 { get; } = new StringBuilder();
        protected StringBuilder builder_fun3 { get; } = new StringBuilder();
        protected StringBuilder builder_return { get; } = new StringBuilder();

        public string Build()
        {
            builder_return.Clear();
            using (var reader = new StreamReader(tempPath))
            {
                do
                {
                    var result = string.Empty;
                    var input = reader.ReadLine();
                    if (input == null) break;

                    //表达式输出
                    if (Regex.IsMatch(input, @"{{(\w+(\[\d\])*)([.]\w+(\[\d\])*)*}}"))
                    {
                        result = Output(ConverFormat(input));
                    }
                    //each开始
                    else if (Regex.IsMatch(input, @"^\s*{{each\s"))
                    {
                        result = Each_start(input);
                    }
                    //each结束
                    else if (Regex.IsMatch(input, @"^\s*{{/each}}\s*$"))
                    {
                        result = Each_end(input);
                    }
                    //if开始
                    else if (Regex.IsMatch(input, @"^\s*{{if\s"))
                    {
                        result = If_start(input);
                    }
                    //else if 开始
                    else if (Regex.IsMatch(input, @"^\s*{{else\sif\s"))
                    {
                        result = ElseIf_start(input);
                    }
                    //if结束
                    else if (Regex.IsMatch(input, @"^\s*{{/if}}\s*$"))
                    {
                        result = If_end();
                    }
                    //空行
                    else if (Regex.IsMatch(input, @"^\s+$"))
                    {
                        result = Empty();
                    }
                    //无表达式追加
                    else
                    {
                        result = Append(input);
                    }
                    builder_return.Append(result);
                } while (true);
            }
            return builder_return.ToString();
        }

        protected string Output(string input)
        {
            ClearResult();
            builder_fun1.Append(input);

            var index = 0;
            do
            {
                var item = Regex.Match(builder_fun1.ToString(), @"{{\w+([.]\w+)?}}");
                if (!item.Success) break;

                builder_fun1.Remove(item.Index, item.Length);
                builder_fun1.Insert(item.Index, string.Format("{{{0}}}", index));

                var vl = ExprConvert(item.Value.Replace("{{", "").Replace("}}", ""));
                builder_fun2.AppendFormat("{0} {1}", ",", vl);
                index++;
            } while (true);

            builder_result.AppendFormat("\nOutput(\"{0}\"{1});", builder_fun1.Replace("\"", "\\\"").ToString(), builder_fun2.ToString());
            return builder_result.ToString();
        }

        protected string Each_start(string input)
        {
            ClearResult();
            var ehval_str = Regex.Match(input, @"(?<={{each\s)(\w+([.]\w+)?)").Value;
            var ehval = ExprConvert(ehval_str);

            var valkey_str = Regex.Match(input, @"(?<={{each\s[\w.]+\s)(\w+(\s\w+)?)").Value;
            var valkeys = valkey_str.Split(' ');
            var val = valkeys[0];
            var key = valkeys[1];

            builder_result.AppendFormat("\nEach({0}, ({1}, {2}) =>", ehval, val, key);
            builder_result.Append("\n{");
            return builder_result.ToString();
        }

        protected string Each_end(string input)
        {
            return "\n});";
        }

        protected string If_start(string input)
        {
            ClearResult();
            builder_result.Append("\nif (");
            var str1 = Regex.Match(input, @"(?<={{if)[\s\S]*(?=}})").Value;
            var loji = Regex.Matches(str1, "&&|\\|\\|").GetEnumerator();
            var sign = Regex.Matches(str1, @"<=|>=|<|>|!=|==").GetEnumerator();

            var list = str1.Split(new string[] { "&&", "||" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(new string[] { "<=", ">=", "<", ">", "!=", "==" }, StringSplitOptions.RemoveEmptyEntries));

            list.ForEach((item, index) =>
            {
                builder_result.AppendFormat("Compare({0}, \"{1}\", {2}) {3} ", ExprConvert(item[0].Trim()), Next(sign).Trim(), ExprConvert(item[1].Trim()), Next(loji).Trim());
            });
            builder_result.Remove(builder_result.Length - 2, 2);
            builder_result.Append(")\n{");
            return builder_result.ToString();
        }

        protected string ElseIf_start(string input)
        {
            ClearResult();
            builder_result.Append("\n}\nelse if (");
            var str1 = Regex.Match(input, @"(?<={{else if)[\s\S]*(?=}})").Value;
            var loji = Regex.Matches(str1, "&&|\\|\\|").GetEnumerator();
            var sign = Regex.Matches(str1, @"<=|>=|<|>|!=|==").GetEnumerator();

            var list = str1.Split(new string[] { "&&", "||" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(new string[] { "<=", ">=", "<", ">", "!=", "==" }, StringSplitOptions.RemoveEmptyEntries));

            list.ForEach((item, index) =>
            {
                builder_result.AppendFormat("Compare({0}, \"{1}\", {2}) {3} ", ExprConvert(item[0].Trim()), Next(sign).Trim(), ExprConvert(item[1].Trim()), Next(loji).Trim());
            });
            builder_result.Remove(builder_result.Length - 2, 2);
            builder_result.Append(")\n{");
            return builder_result.ToString();
        }

        protected string If_end()
        {
            return "\n}";
        }

        protected string Append(string input)
        {
            ClearResult();
            builder_result.AppendFormat("\nAppend(\"{0}\");", input);
            return builder_result.ToString();
        }

        protected string Empty()
        {
            return "\nAppend();";
        }

        private string ExprConvert(string expression)
        {
            exprBuilder.Clear();
            var values = expression.Split('.');

            exprBuilder.Append(values[0]);
            for (int i = 1; i < values.Length; i++)
            {
                if (Regex.IsMatch(values[i], @"^\w+\[\d+\]$"))
                {
                    var start = Regex.Match(values[i], @"\w+").Value;
                    var end = Regex.Match(values[i], @"\[\d+\]").Value;
                    exprBuilder.AppendFormat("[\"{0}\"]", start, end);
                }
                else
                {
                    exprBuilder.AppendFormat("[\"{0}\"]", values[i]);
                }
            }
            return exprBuilder.ToString();
        }


        private void ClearResult()
        {
            builder_result.Clear();
            builder_fun1.Clear();
            builder_fun2.Clear();
            builder_fun3.Clear();
        }

        private string ConverFormat(string input)
        {
            var mt = Regex.Match(input, @"(?<!{)({)(?!{)");

            var str = Regex.Replace(input, @"(?<!{)({)(?!{)", "{{");
            return Regex.Replace(str, @"(?<!})(})(?!})", "}}");
        }

        private string Next(IEnumerator enumerator)
        {
            var result = string.Empty;
            if (enumerator.MoveNext())
            {
                result = enumerator.Current.ToString();
            }
            return result;
        }
    }

}
