using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Azure.Storage.Blobs.Models;

using NeuroMedia.Application.Features.Questionnaires.Dtos;

namespace NeuroMedia.WebUI.Helpers
{
    public static class UtilityHelpers
    {
        public static int CalculateYears(DateTime date)
        {
            var today = DateTime.Today;
            var age = today.Year - date.Year;
            if (date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

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

        public static string GetDisplayName(this Enum enumValue)
        {
            var enumMember = enumValue.GetType().GetMember(enumValue.ToString()).First();
            var displayAttribute = enumMember.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.GetType().GetMember(e.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
            }).ToList();
        }

        public static string GetAnswerString(QuestionDto question, AnswerDto answer)
        {
            switch (question.Type)
            {
                case "TextInput":
                    return answer.ResultValue;

                case "Radio":
                case "DropDown":
                    var option = question.Options.FirstOrDefault(o => o.Id == answer.OptionId);
                    return option != null ? option.Text : "Invalid option";

                default:
                    return "Invalid option";
            }
        }
    }

    public class SelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
