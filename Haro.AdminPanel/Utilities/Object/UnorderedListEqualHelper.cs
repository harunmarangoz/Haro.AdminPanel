using System.Collections.Generic;

namespace Haro.AdminPanel.Utilities.Object
{
    public class UnorderedListEqualHelper
    {
        public static bool UnorderedEqual<T>(ICollection<T> a, ICollection<T> b)
        {
            if (a.Count != b.Count)
                return false;
            Dictionary<T, int> dictionary = new Dictionary<T, int>();
            foreach (T key in (IEnumerable<T>) a)
            {
                int num;
                if (dictionary.TryGetValue(key, out num))
                    dictionary[key] = num + 1;
                else
                    dictionary.Add(key, 1);
            }
            foreach (T key in (IEnumerable<T>) b)
            {
                int num;
                if (!dictionary.TryGetValue(key, out num) || num == 0)
                    return false;
                dictionary[key] = num - 1;
            }
            foreach (int num in dictionary.Values)
            {
                if (num != 0)
                    return false;
            }
            return true;
        }
    }
}