using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroMedia.Application.Common.Validations;

namespace NeuroMedia.Application.Tests.Common.Validations
{
    public class NotInFutureAttributeTests
    {
        private readonly NotInFutureAttribute _notInFutureAttribute = new();

        [Fact]
        public void IsValid_Today_ReturnsTrue()
        {
            var today = DateTime.Today;
            var result = _notInFutureAttribute.IsValid(today);

            Assert.True(result);
        }
        [Fact]
        public void IsValid_PastDate_ReturnsTrue()
        {
            var pastDate = DateTime.Today.AddDays(-1);
            var result = _notInFutureAttribute.IsValid(pastDate);

            Assert.True(result);
        }
        [Fact]
        public void IsValid_FutureDate_ReturnsFalse()
        {
            var futureDate = DateTime.Today.AddDays(1);
            var result = _notInFutureAttribute.IsValid(futureDate);

            Assert.False(result);
        }
        [Fact]
        public void IsValid_Null_ReturnsTrue()
        {
            DateTime? nullDate = null;
            var result = _notInFutureAttribute.IsValid(nullDate);

            Assert.True(result);
        }
    }
}
