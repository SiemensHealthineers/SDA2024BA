using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Features.Visits.Commands.CreateVisit;

namespace NeuroMedia.Application.Tests.Features.Visits.Commands.CreateVisit
{
    public class CreateVisitDtoTests
    {
        private static IList<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }
        [Fact]
        public void Validation_Passes_WhenAllFieldsAreValid()
        {
            //Arrange
            var dto = new CreateVisitDto()
            {
                DateOfVisit = DateTime.Now,
                PatientId = 1,
                VisitType = 0
            };
            //Act
            var results = ValidateModel(dto);
            //Assert
            Assert.Empty(results);
        }
    }
}
