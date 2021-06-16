using System.Collections.Generic;

namespace NumericExpressionEngine
{
    internal static class Extensions
    {
        internal static void AddOrUpdate(this Dictionary<string, int> dic, string key, int value)
        {
            if (!dic.ContainsKey(key))
                dic.Add(key, value);
            else
                dic[key] = value;
        }
    }
}
