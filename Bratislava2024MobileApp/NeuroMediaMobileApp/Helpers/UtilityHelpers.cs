using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Maui.Controls;

namespace NeuroMediaMobileApp.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var enumMember = enumValue.GetType().GetMember(enumValue.ToString()).First();
            var displayAttribute = enumMember.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }

    public class BooleanToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? "Yes" : "No";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (stringValue.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (stringValue.Equals("No", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return null;
        }
    }

    public class CalculateDisease
    {
        public static string CalculateDiseaseDuration(DateTime date)
        {
            var today = DateTime.Today;

            if (date > today || date == today)
            {
                return "0 days";
            }

            var years = today.Year - date.Year;
            var months = today.Month - date.Month;
            var days = today.Day - date.Day;

            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(today.Year, today.AddMonths(-1).Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            var parts = new List<string>();
            if (years > 0)
            {
                parts.Add($"{years} year{(years > 1 ? "s" : "")}");
            }
            if (months > 0)
            {
                parts.Add($"{months} month{(months > 1 ? "s" : "")}");
            }
            if (days > 0)
            {
                parts.Add($"{days} day{(days > 1 ? "s" : "")}");
            }
            return string.Join(", ", parts);
        }
    }
}
