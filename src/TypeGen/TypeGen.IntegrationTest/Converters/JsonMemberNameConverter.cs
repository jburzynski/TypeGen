using System.Reflection;
using Newtonsoft.Json;
using TypeGen.Core.Converters;

namespace TypeGen.IntegrationTest.Converters
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