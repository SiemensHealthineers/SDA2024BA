using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using NeuroMedia.WebUI.Pages;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using Moq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading.Tasks;
using Moq.Protected;
using NeuroMedia.WebUI.Services.Interfaces;


namespace NeuroMedia.WebUI.Tests.Components
{
    public class EditPatientModalTests : TestContext
    {
        private readonly Mock<IHttpClientService> _httpClientServiceMock;

        public EditPatientModalTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
            Services.AddSingleton(_httpClientServiceMock.Object);
        }

        [Fact]
        public void ModalDisplaysPatientData()
        {
            // Arrange
            var patientData = new GetPatientByIdDto
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                Sex = Domain.Enums.Sex.Female,
                Email = "john.doe@example.com",
                Diagnosis = Domain.Enums.Diagnoses.CervicalDystonia,
                IsActive = true
            };

            // Act
            var cut = RenderComponent<EditPatientModal>(parameters => parameters
                .Add(p => p.InputPatient, patientData));

            // Assert
            cut.Find("#inputName").MarkupMatches(@"<input id=""inputName"" placeholder=""Name"" name=""patient.Name"" class=""form-control valid"" value=""John""  >");
            cut.Find("#inputSurname").MarkupMatches(@"<input id=""inputSurname"" placeholder=""Surname"" name=""patient.Surname"" class=""form-control valid"" value=""Doe""  >");
            cut.Find("#inputEmail").MarkupMatches(@"<input type=""email"" id=""inputEmail"" placeholder=""Email"" name=""patient.Email"" class=""form-control valid"" value=""john.doe@example.com""  >");
            cut.Find("#inputDate").MarkupMatches(@"<input id=""inputDate"" type=""date"" name=""patient.DateOfBirth"" class=""form-control valid"" value=""1980-01-01""  >");
            cut.Find("#inputSex");
            cut.Find("#inputDiagnosis");
            cut.Find("#inputEducation");
            cut.Find("#inputPhone");
            cut.Find("#inputEducation");
            cut.Find("#inputDiagDate");
            cut.Find("#inputEmployment");
            cut.Find("#RAPexam");
            cut.Find("#applicationCheck");
            cut.Find(".deactivate_btn");
        }
        [Fact]
        public void ExtractPatientData_ShouldReturnCorrectPatientData()
        {
            // Arrange
            var inputPatient = new GetPatientByIdDto
            {
                Name = "John",
                Surname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Sex = Domain.Enums.Sex.Male,
                Diagnosis = Domain.Enums.Diagnoses.OtherDiagnosis,
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                HighestEducation = Domain.Enums.EducationLevel.UniversityUpToBachelorsLevel,
                EmploymentStatus = Domain.Enums.EmploymentStatus.SelfEmployed,
                RapExamination = false,
                PreviousBotulinumToxinApplication = true,
                IsActive = true,
                DateOfDiagnosis = new DateTime(2020, 1, 1)
            };
            var component = new EditPatientModal();

            // Act
            var result = component.ExtractPatientData(inputPatient);

            // Assert
            Assert.Equal(inputPatient.Name, result.Name);
            Assert.Equal(inputPatient.Surname, result.Surname);
            Assert.Equal(inputPatient.DateOfBirth, result.DateOfBirth);
            Assert.Equal(inputPatient.Sex, result.Sex);
            Assert.Equal(inputPatient.Diagnosis, result.Diagnosis);
            Assert.Equal(inputPatient.Email, result.Email);
            Assert.Equal(inputPatient.PhoneNumber, result.PhoneNumber);
            Assert.Equal(inputPatient.HighestEducation, result.HighestEducation);
            Assert.Equal(inputPatient.EmploymentStatus, result.EmploymentStatus);
            Assert.Equal(inputPatient.RapExamination, result.RapExamination);
            Assert.Equal(inputPatient.PreviousBotulinumToxinApplication, result.PreviousBotulinumToxinApplication);
            Assert.Equal(inputPatient.IsActive, result.IsActive);
            Assert.Equal(inputPatient.DateOfDiagnosis, result.DateOfDiagnosis);
        }
    }
}
