using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class ListExtension
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action.Invoke(item);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            var index = 0;
            foreach (var item in source)
            {
                action.Invoke(item, index);
                ++index;
            }
        }

        public static void ForEach<TSource>(this List<TSource> source, Action<TSource, int> action)
        {
            var index = 0;
            foreach (var item in source)
            {
                action.Invoke(item, index);
                ++index;
            }
        }

        public static void ForEach(this MatchCollection match, Action<Match, int> action)
        {
            var index = 0;
            foreach (Match item in match)
            {
                action.Invoke(item, index);
                ++index;
            }
        }
    }
}
