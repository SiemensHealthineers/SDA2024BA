using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.WebUI.Helpers;

namespace NeuroMedia.WebUI.Tests.Helpers
{
    public class UtilityHelpersTests
    {
        [Fact]
        public void CalculateYears_BirthdayToday_ReturnsCorrectAge()
        {
            var dateOfBirth = DateTime.Today.AddYears(-30);

            var age = UtilityHelpers.CalculateYears(dateOfBirth);

            Assert.Equal(30, age);
        }

        [Fact]
        public void CalculateYears_BirthdayYesterday_ReturnsCorrectAge()
        {
            var dateOfBirth = DateTime.Today.AddYears(-30).AddDays(-1);

            var age = UtilityHelpers.CalculateYears(dateOfBirth);

            Assert.Equal(30, age);
        }

        [Fact]
        public void CalculateYears_BirthdayTomorrow__ReturnsCorrectAge()
        {
            var dateOfBirth = DateTime.Today.AddYears(-30).AddDays(1);

            var age = UtilityHelpers.CalculateYears(dateOfBirth);

            Assert.Equal(29, age);
        }

        [Fact]
        public void CalculateYears_SameDateForBirthAndToday__ReturnsZero()
        {
            var dateOfBirth = DateTime.Today;

            var age = UtilityHelpers.CalculateYears(dateOfBirth);

            Assert.Equal(0, age);
        }

        [Fact]
        public void CalculateDiseaseDuration_ForExactYear_ReturnsCorrectDuration()
        {
            var date = DateTime.Today.AddYears(-1);

            var result = UtilityHelpers.CalculateDiseaseDuration(date);

            Assert.Equal("1 year", result);
        }

        [Fact]
        public void CalculateDiseaseDuration_ForExactDay_ReturnsCorrectDuration()
        {
            var date = DateTime.Today.AddDays(-1);

            var result = UtilityHelpers.CalculateDiseaseDuration(date);

            Assert.Equal("1 day", result);
        }

        [Fact]
        public void CalculateDiseaseDuration_ForFutureDay_ReturnsCorrectDuration()
        {
            var date = DateTime.Today.AddDays(1);

            var result = UtilityHelpers.CalculateDiseaseDuration(date);

            Assert.Equal("0 days", result);
        }

        [Fact]
        public void CalculateDiseaseDuration_ForMultipleYearsMonths_ReturnsCorrectDuration()
        {
            var date = new DateTime(DateTime.Today.Year - 2, DateTime.Today.Month - 3, DateTime.Today.Day);

            var result = UtilityHelpers.CalculateDiseaseDuration(date);

            Assert.Equal("2 years, 3 months", result);
        }


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
            var result = UtilityHelpers.GetEnumSelectList<TestEnumType>().ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal("FirstValue", result[0].Value);
            Assert.Equal("First Value", result[0].Text);
            Assert.Equal("SecondValue", result[1].Text);
            Assert.Equal("ThirdValue", result[2].Value);
            Assert.Equal("Third Value", result[2].Text);
        }

        [Fact]
        public void GetEnumSelectList_ReturnsSelectListItems()
        {
            var expectedItems = new List<SelectListItem>
            {
                new() { Value = "FirstValue", Text = "First Value" },
                new() { Value = "SecondValue", Text = "SecondValue" },
                new() { Value = "ThirdValue", Text = "Third Value" }
            };

            var selectListItems = UtilityHelpers.GetEnumSelectList<TestEnumType>().ToList();

            Assert.Equal(expectedItems.Count, selectListItems.Count);
            foreach (var expected in expectedItems)
            {
                var actual = selectListItems.FirstOrDefault(x => x.Value == expected.Value);
                Assert.NotNull(actual);
                Assert.Equal(expected.Text, actual.Text);
            }
        }
    }
}
