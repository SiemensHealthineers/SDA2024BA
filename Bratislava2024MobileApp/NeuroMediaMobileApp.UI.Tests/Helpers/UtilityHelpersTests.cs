using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NeuroMediaMobileApp.Helpers;
using Xunit;
using System.Globalization;


namespace NeuroMediaMobileApp.UI.Tests.Helpers
{
    public class UtilityHelpersTests
    {
        public enum TestEnumType
        {
            [Display(Name = "First Value")]
            FirstValue,
            SecondValue,
            [Display(Name = "Third Value")]
            ThirdValue
        }

        [Fact]
        public void GetDisplayName_WhenDisplayAttributeIsPresent_ReturnsDisplayName()
        {
            var result = TestEnumType.FirstValue.GetDisplayName();

            Assert.Equal("First Value", result);
        }

        [Fact]
        public void GetDisplayName_WhenDisplayAttributeIsNotPresent_ReturnsEnumValue()
        {
            var result = TestEnumType.SecondValue.GetDisplayName();

            Assert.Equal("SecondValue", result);
        }

        [Fact]
        public void GetEnumSelectList_ReturnsCorrectList()
        {
            var enumValues = Enum.GetValues(typeof(TestEnumType)).Cast<TestEnumType>().ToList();

            var result = enumValues.Select(e => new { Value = e.ToString(), DisplayName = e.GetDisplayName() }).ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal("FirstValue", result[0].Value);
            Assert.Equal("First Value", result[0].DisplayName);
            Assert.Equal("SecondValue", result[1].DisplayName);
            Assert.Equal("ThirdValue", result[2].Value);
            Assert.Equal("Third Value", result[2].DisplayName);
        }
    }

    public class BooleanToYesNoConverterTests
    {
        [Theory]
        [InlineData(true, "Yes")]
        [InlineData(false, "No")]
        public void Convert_ShouldReturnYesOrNo_ForBooleanValues(bool input, string expected)
        {
            var converter = new BooleanToYesNoConverter();
            var result = converter.Convert(input, typeof(string), null, CultureInfo.InvariantCulture);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Yes", true)]
        [InlineData("No", false)]
        [InlineData("yes", true)]
        [InlineData("no", false)]
        [InlineData("InvalidValue", null)]
        public void ConvertBack_ShouldReturnExpectedBoolean(string input, bool? expected)
        {
            var converter = new BooleanToYesNoConverter();

            var result = converter.ConvertBack(input, typeof(bool), null, CultureInfo.InvariantCulture);

            var actualResult = result as bool?;

            Assert.Equal(expected, actualResult);
        }

        [Fact]
        public void Convert_ShouldReturnNull_ForInvalidInput()
        {
            var converter = new BooleanToYesNoConverter();
            var result = converter.Convert("InvalidType", typeof(string), null, CultureInfo.InvariantCulture);
            Assert.Null(result);
        }

        [Fact]
        public void ConvertBack_ShouldReturnNull_ForInvalidInput()
        {
            var converter = new BooleanToYesNoConverter();
            var result = converter.ConvertBack(123, typeof(bool), null, CultureInfo.InvariantCulture);
            Assert.Null(result);
        }
    }

    public class CalculateDisease
    {
        [Fact]
        public void CalculateDiseaseDuration_ForExactYear_ReturnsCorrectDuration()
        {
            var date = DateTime.Today.AddYears(-1);

            var result = NeuroMediaMobileApp.Helpers.CalculateDisease.CalculateDiseaseDuration(date);

            Assert.Equal("1 year", result);
        }

        [Fact]
        public void CalculateDiseaseDuration_ForExactDay_ReturnsCorrectDuration()
        {
            var date = DateTime.Today.AddDays(-1);

            var result = NeuroMediaMobileApp.Helpers.CalculateDisease.CalculateDiseaseDuration(date);

            Assert.Equal("1 day", result);
        }

        [Fact]
        public void CalculateDiseaseDuration_ForFutureDay_ReturnsCorrectDuration()
        {
            var date = DateTime.Today.AddDays(1);

            var result = NeuroMediaMobileApp.Helpers.CalculateDisease.CalculateDiseaseDuration(date);

            Assert.Equal("0 days", result);
        }

        [Fact]
        public void CalculateDiseaseDuration_ForMultipleYearsMonths_ReturnsCorrectDuration()
        {
            var date = new DateTime(DateTime.Today.Year - 2, DateTime.Today.Month - 3, DateTime.Today.Day);

            var result = NeuroMediaMobileApp.Helpers.CalculateDisease.CalculateDiseaseDuration(date);

            Assert.Equal("2 years, 3 months", result);
        }
    }
}
