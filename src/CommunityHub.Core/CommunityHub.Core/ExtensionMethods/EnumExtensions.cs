using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace CommunityHub.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumMemberValue(this Enum value) =>
            value.GetType()
                 .GetField(value.ToString())
                 .GetCustomAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();

        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}