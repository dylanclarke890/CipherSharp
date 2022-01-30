using System.Collections.Generic;

namespace CipherSharp.Attacks.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<T>(this Dictionary<T, int> dict, T value)
        {
            if (dict.ContainsKey(value))
            {
                dict[value] += 1;
            }
            else
            {
                dict.Add(value, 1);
            }
        }
    }
}
