using System.Reflection;
using Newtonsoft.Json;
using TypeGen.Core.Converters;

namespace TypeGen.FileContentTest.Converters
{
    public class JsonMemberNameConverter : IMemberNameConverter
    {
        public string Convert(string name, MemberInfo memberInfo)
        {
            var attribute = memberInfo.GetCustomAttribute<JsonPropertyAttribute>();
            return attribute != null ? attribute.PropertyName : name;
        }
    }
}