using System.Reflection;

namespace TypeGen.Core.Converters
{
    /// <summary>
    /// Interface for member name converters
    /// </summary>
    public interface IMemberNameConverter : IConverter
    {
        /// <summary>
        /// Converts a member name to a new name.
        /// Conversion can optionally depend on the MemberInfo of the member, which name is being changed.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="memberInfo">The MemberInfo of the member, which name is being changed</param>
        /// <returns>Converted name</returns>
        string Convert(string name, MemberInfo memberInfo);
    }
}
