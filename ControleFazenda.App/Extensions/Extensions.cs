using System.ComponentModel.DataAnnotations;

namespace ControleFazenda.App.Extensions
{
    public static class Extensions
    {
        //public static string GetDisplayName(this Enum enumValue)
        //{
        //    var memberInfo = enumValue.GetType().GetMember(enumValue.ToString());
        //    if (memberInfo.Length > 0)
        //    {
        //        var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(memberInfo[0], typeof(DisplayAttribute));
        //        if (displayAttribute != null)
        //        {
        //            return displayAttribute.Name;
        //        }
        //    }
        //    return enumValue.ToString();
        //}


        public static string? GetEnumDisplayName(this Enum enumValue)
        {
            var displayAttribute = enumValue?.GetType()?.GetField(enumValue.ToString())?.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            return displayAttribute != null && displayAttribute.Length > 0 ? displayAttribute[0].Name : enumValue?.ToString();
        }
    }
}
