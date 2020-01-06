using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TempTop
{
    /// <summary>
    /// 根据C#代码抽象类
    /// </summary>
    public abstract class TempBase : ITempBuild
    {
        public readonly StringBuilder builder = new StringBuilder();
        protected dynamic _data { get; private set; }

        public TempBase() { }

        #region 加载data
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

        #endregion

        #region 生成逻辑

        /// <summary>
        /// 表达式输出
        /// </summary>
        /// <param name="token"></param>
        /// <param name="formate"></param>
        /// <param name="path"></param>
        protected void Output(string formate, params dynamic[] values)
        {
            this.builder.AppendLine(string.Format(formate, values));
        }

        /// <summary>
        /// 追加文本
        /// </summary>
        /// <param name="value"></param>
        protected void Append(string value = "")
        {
            this.builder.AppendLine(value);
        }

        /// <summary>
        /// 循环输出
        /// </summary>
        /// <param name="jsonpath"></param>
        /// <param name="action"></param>
        protected void Each(JToken token, Action<dynamic, int> action)
        {
            token.ForEach(action);
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void Clear()
        {
            this.builder.Clear();
        }

        /// <summary>
        /// 生成逻辑
        /// </summary>
        protected abstract void Invoke();

        #endregion

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            Clear();
            Invoke();
            builder.Remove(builder.Length - 2, 2);
            return builder.ToString();
        }

    }
}
