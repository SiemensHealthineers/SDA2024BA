using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using Bunit;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Moq.Protected;

using NeuroMedia.Application.Features.Patients.Commands.CreatePatient;
using NeuroMedia.WebUI.Models;
using NeuroMedia.WebUI.Pages;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Tests.Components
{
    public class TestableAddPatientModal : AddPatientModal
    {
        public new void OnInitialized()
        {
            base.OnInitialized();
        }

        public PatientDataTransfer GetPatient()
        {
            return this.Patient;
        }
    }

    public class AddPatientModalTests : TestContext
    {
        private readonly Mock<IHttpClientService> _httpClientServiceMock;

        public AddPatientModalTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
            Services.AddSingleton(_httpClientServiceMock.Object);
        }

        [Fact]
        public void FormValidation_NoName_ShowsValidationMessage()
        {
            var component = new AddPatientModalTests().RenderComponent<AddPatientModal>();

            component.Find("#inputName").Change("");
            component.Find("form").Submit();

            var validationMessage = component.Find("div.validation-message");
            Assert.Contains("Name is required.", validationMessage.TextContent);
        }

        [Theory]
        [InlineData("invalid-email", "1234567890", "Invalid email address.")]
        [InlineData("john.doe@example.com", "phoneNum", "Invalid phone number.")]
        public void FormValidation_ShowsValidationMessage(string email, string phone, string expectedValidationMessage)
        {
            var component = new AddPatientModalTests().RenderComponent<AddPatientModal>();

            component.Find("#inputName").Change("John");
            component.Find("#inputSurname").Change("Doe");
            component.Find("#inputDate").Change(DateTime.Today.ToString("yyyy-MM-dd"));
            component.Find("#inputPhone").Change(phone);
            component.Find("#inputSex").Change("Male");
            component.Find("#inputEmail").Change(email);
            component.Find("form").Submit();

            var validationMessage = component.Find("div.validation-message");
            Assert.Contains(expectedValidationMessage, validationMessage.TextContent);
        }

        [Fact]
        public void OnInitialized_SetsDefaultDates()
        {
            var component = new TestableAddPatientModal();

            component.OnInitialized();

            Assert.Equal(DateTime.Today, component.Patient.DateOfBirth);
            Assert.Equal(DateTime.Today, component.Patient.DateOfDiagnosis);
        }

    }
}
