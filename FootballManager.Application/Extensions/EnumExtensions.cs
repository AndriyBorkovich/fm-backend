using System.ComponentModel;
using System.Reflection;

namespace FootballManager.Application.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumOption)
        => enumOption.GetAttributeOfType<DescriptionAttribute>().Description;

    public static T GetAttributeOfType<T>(this Enum enumValue) where T : Attribute
    {
        var type = enumValue.GetType();
        var memInfo = type.GetMember(enumValue.ToString()).First();
        var attributes = memInfo.GetCustomAttributes<T>(false);
        return attributes.FirstOrDefault();
    }
}
