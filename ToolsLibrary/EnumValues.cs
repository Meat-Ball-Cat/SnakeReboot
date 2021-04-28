using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsLibrary
{
    public static class EnumValues
    {
        public static object[] GetValues(Type enumType)
        {
            var result = new List<object>();
            if (enumType.IsEnum)
            {
                foreach (var x in Enum.GetValues(enumType))
                {
                    result.Add(x);
                }
            }
            return result.ToArray();
        }
        public static object Next(Enum value)
        {
            var values = GetValues(value.GetType());
            for(int i = 0; i < values.Length; i++)
            {
                if(value.Equals(values[i]))
                {
                    i = (i + 1) % values.Length;
                    return values[i];
                }
            }
            return null; ;
        }
    }
}
