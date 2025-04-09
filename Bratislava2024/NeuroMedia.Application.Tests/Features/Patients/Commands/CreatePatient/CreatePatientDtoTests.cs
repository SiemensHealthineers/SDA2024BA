using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroMedia.Application.Features.Patients.Commands.CreatePatient;

namespace NeuroMedia.Application.Tests.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientDtoTests
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
            var dto = new CreatePatientDto()
            {
                Name = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-10),
                Sex = Domain.Enums.Sex.Male,
                Diagnosis = Domain.Enums.Diagnoses.OtherDiagnosis,
                DateOfDiagnosis = DateTime.Now.AddDays(-10),
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                HighestEducation = Domain.Enums.EducationLevel.SecondarySchoolWithDiploma,
                EmploymentStatus = Domain.Enums.EmploymentStatus.Retired,
                RapExamination = false,
                PreviousBotulinumToxinApplication = true
            };
            var results = ValidateModel(dto);
            Assert.Empty(results);
        }
        [Fact]
        public void NameRequiredValidation_Fails_WhenNull()
        {
            var dto = new CreatePatientDto()
            {
                Name = null,
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-10),
                Sex = Domain.Enums.Sex.Male,
                Diagnosis = Domain.Enums.Diagnoses.OtherDiagnosis,
                DateOfDiagnosis = DateTime.Now.AddDays(-10),
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                HighestEducation = Domain.Enums.EducationLevel.SecondarySchoolWithDiploma,
                EmploymentStatus = Domain.Enums.EmploymentStatus.Retired,
                RapExamination = false,
                PreviousBotulinumToxinApplication = true
            };
            var results = ValidateModel(dto);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(dto.Name)) && v.ErrorMessage.Contains("required"));
        }
        [Fact]
        public void NameRequiredValidation_Fails_WhenEmpty()
        {
            var dto = new CreatePatientDto()
            {
                Name = "",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-10),
                Sex = Domain.Enums.Sex.Male,
                Diagnosis = Domain.Enums.Diagnoses.OtherDiagnosis,
                DateOfDiagnosis = DateTime.Now.AddDays(-10),
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                HighestEducation = Domain.Enums.EducationLevel.SecondarySchoolWithDiploma,
                EmploymentStatus = Domain.Enums.EmploymentStatus.Retired,
                RapExamination = false,
                PreviousBotulinumToxinApplication = true
            };
            var results = ValidateModel(dto);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(dto.Name)) && v.ErrorMessage.Contains("required"));
        }
        [Fact]
        public void EmailValidation_Fails_WhenInvalid()
        {
            var dto = new CreatePatientDto()
            {
                Name = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-10),
                Sex = Domain.Enums.Sex.Male,
                Diagnosis = Domain.Enums.Diagnoses.OtherDiagnosis,
                DateOfDiagnosis = DateTime.Now.AddDays(-10),
                Email = "john.doe",
                PhoneNumber = "1234567890",
                HighestEducation = Domain.Enums.EducationLevel.SecondarySchoolWithDiploma,
                EmploymentStatus = Domain.Enums.EmploymentStatus.Retired,
                RapExamination = false,
                PreviousBotulinumToxinApplication = true
            };
            var results = ValidateModel(dto);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(dto.Email)) && v.ErrorMessage.Contains("valid e-mail"));
        }
        [Fact]
        public void PhoneValidation_Fails_WhenInvalid()
        {
            var dto = new CreatePatientDto()
            {
                Name = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-10),
                Sex = Domain.Enums.Sex.Male,
                Diagnosis = Domain.Enums.Diagnoses.OtherDiagnosis,
                DateOfDiagnosis = DateTime.Now.AddDays(-10),
                Email = "john.doe@example.com",
                PhoneNumber = "string",
                HighestEducation = Domain.Enums.EducationLevel.SecondarySchoolWithDiploma,
                EmploymentStatus = Domain.Enums.EmploymentStatus.Retired,
                RapExamination = false,
                PreviousBotulinumToxinApplication = true
            };
            var results = ValidateModel(dto);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(dto.PhoneNumber)) && v.ErrorMessage.Contains("a valid phone number"));
        }
        [Fact]
        public void DateOfBirthValidation_Fails_WhenFuture()
        {
            var dto = new CreatePatientDto()
            {
                Name = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddDays(1),
                Sex = Domain.Enums.Sex.Male,
                Diagnosis = Domain.Enums.Diagnoses.OtherDiagnosis,
                DateOfDiagnosis = DateTime.Now.AddDays(-10),
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                HighestEducation = Domain.Enums.EducationLevel.SecondarySchoolWithDiploma,
                EmploymentStatus = Domain.Enums.EmploymentStatus.Retired,
                RapExamination = false,
                PreviousBotulinumToxinApplication = true
            };
            var results = ValidateModel(dto);
            Assert.Contains(results, v => v.MemberNames.Contains(nameof(dto.DateOfBirth)) && v.ErrorMessage.Contains("the future"));
        }
    }
}
