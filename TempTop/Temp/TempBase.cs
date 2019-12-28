using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TempTop
{
    public abstract class TempBase
    {

        protected JObject _data { get; set; }
        protected StringBuilder builder { get; }
        public string outputPath { get; set; } = @"C:\Users\Administrator\Desktop\model.cs";

        #region 构造
        public TempBase()
        {
            builder = new StringBuilder();
        }

        public TempBase(object obj) : this()
        {
            _data = JObject.FromObject(obj);
        }

        public TempBase(string json) : this()
        {
            _data = JObject.Parse(json);
        }

        public void LoadFromFile(string path)
        {
            _data = JObject.Parse(Reader(path));
            Clear();
        }

        public void LoadFromJson(string json)
        {
            _data = JObject.Parse(json);
            Clear();
        }

        public void LoadFromObject(object obj)
        {
            _data = JObject.FromObject(obj);
            Clear();
        }
        #endregion

        /// <summary>
        /// 读
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string Reader(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 写
        /// </summary>
        /// <param name="path"></param>
        private void Writer(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(this.builder.ToString());
            }
        }

        /// <summary>
        /// 表达式输出
        /// </summary>
        /// <param name="token"></param>
        /// <param name="formate"></param>
        /// <param name="path"></param>
        protected void Output(string formate, params JToken[] values)
        {
            var strs = values.Select(x => x.Value<string>()).ToArray();
            this.builder.AppendLine(string.Format(formate, values));
        }

        /// <summary>
        /// 追加文本
        /// </summary>
        /// <param name="value"></param>
        protected void Append(string value = default)
        {
            this.builder.AppendLine(value);
        }

        /// <summary>
        /// 循环输出
        /// </summary>
        /// <param name="jsonpath"></param>
        /// <param name="action"></param>
        protected void Each(JToken token, Action<JToken, int> action)
        {
            token.ForEach(action);
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected bool Compare(object obj1, string sign, object obj2)
        {
            bool result = false;
            JToken token1 = JToken.FromObject(obj1);
            JToken token2 = JToken.FromObject(obj2);
            switch (sign)
            {
                case "!=":
                    result = token1.ToString() != token2.ToString();
                    break;
                case "==":
                    result = token1.ToString() == token2.ToString();
                    break;
                case "<=":
                    result = Convert.ToDecimal(token1) <= Convert.ToDecimal(token2);
                    break;
                case ">=":
                    result = Convert.ToDecimal(token1) >= Convert.ToDecimal(token2);
                    break;
                case "<":
                    result = Convert.ToDecimal(token1) < Convert.ToDecimal(token2);
                    break;
                case ">":
                    result = Convert.ToDecimal(token1) > Convert.ToDecimal(token2);
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 生成逻辑
        /// </summary>
        protected abstract void Invoke();

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public void Execute()
        {
            Clear();
            LoadFromFile(@"C:\Users\Administrator\source\repos\TempTop\TempTop\Temp\模板数据.json");
            Invoke();
            builder.Remove(builder.Length - 2, 2);
            Writer(outputPath);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this.builder.Clear();
        }
    }
}
