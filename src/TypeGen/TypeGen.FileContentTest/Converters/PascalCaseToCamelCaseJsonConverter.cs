using System.Reflection;
using Newtonsoft.Json;
using TypeGen.Core.Converters;

namespace TypeGen.FileContentTest.Converters
{
    public class PascalCaseToCamelCaseJsonConverter : IMemberNameConverter
    {
        public string Convert(string name, MemberInfo memberInfo)
        {
            var attribute = memberInfo.GetCustomAttribute<JsonPropertyAttribute>();
            if (attribute != null) return name;
            
            char firstChar = char.ToLowerInvariant(name[0]);
            return firstChar + name.Remove(0, 1);
        }
    }
}