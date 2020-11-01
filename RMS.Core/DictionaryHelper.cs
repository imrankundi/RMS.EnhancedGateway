using System.Collections.Generic;

namespace RMS.Core
{
    public class DictionaryHelper
    {
        public static bool KeyExist(string key, Dictionary<object, object> dictionary)
        {
            if (dictionary.ContainsKey(key))
                return true;

            return false;
        }
    }
}
