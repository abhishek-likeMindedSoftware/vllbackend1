using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Components.Shared
{
    /// <summary>
    /// Gets the [Description] attribute value for an enum member.
    /// Falls back to the enum name with underscores replaced by spaces if no Description is set.
    /// </summary>
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString().Replace("_", " ");

            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString().Replace("_", " ");
        }

        public static IEnumerable<(T Value, string Label)> GetOptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(v => (v, GetDescription(v)));
        }

        /// <summary>
        /// Returns a Yes/No badge as a MarkupString, consistent with the ll-badge system.
        /// Usage in Razor: @EnumHelper.BoolBadge(value)
        /// </summary>
        public static MarkupString BoolBadge(bool value)
        {
            var css  = value ? "ll-badge ll-badge-decision_issued" : "ll-badge ll-badge-incomplete";
            var text = value ? "Yes" : "No";
            return new MarkupString($"<span class=\"{css}\">{text}</span>");
        }
    }
}
