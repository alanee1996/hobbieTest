using System;
using Newtonsoft.Json;

namespace hobbie.Utilis
{
    public static class Extensions
    {
        public static string combine(this string value, params object[] objs)
        {
            try
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    if (IsDataType(objs[i]))
                    {
                        value = value.Replace("{" + i + "}", objs[i].ToString());
                    }
                    else
                        value = value.Replace("{" + i + "}", JsonConvert.SerializeObject(objs[i]));
                }

                return value;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static bool IsDataType(object obj)
        {
            if (obj is int) return true;
            if (obj is Int16) return true;
            if (obj is Int32) return true;
            if (obj is Int64) return true;
            if (obj is string) return true;
            if (obj is double) return true;
            if (obj is decimal) return true;
            if (obj is char) return true;

            return false;
        }
    }
}
