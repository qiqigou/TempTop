﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TempTop
{
    /// <summary>
    /// 根据C#代码抽象类
    /// </summary>
    public abstract class TempBase : ITempBuild
    {
        public StringBuilder builder { get; } = new StringBuilder();
        protected JObject _data { get; private set; }

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