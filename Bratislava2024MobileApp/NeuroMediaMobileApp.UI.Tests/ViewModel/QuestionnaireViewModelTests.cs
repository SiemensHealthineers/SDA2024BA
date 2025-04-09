using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using NeuroMediaMobileApp.Models.DTOs;
using NeuroMediaMobileApp.Models.Enums;
using NeuroMediaMobileApp.Services.Interfaces;
using NeuroMediaMobileApp.ViewModel;

namespace NeuroMediaMobileApp.UI.Tests.ViewModel
{
    public class QuestionnaireViewModelTests
    {
        private readonly Mock<INavigationService> _mockNavigationService;
        private readonly Mock<IAlertService> _mockAlertService;
        private readonly Mock<IQuestionnaireService> _mockQuestionnaireService;
        private readonly Mock<ISessionService> _mockSessionService;
        private readonly QuestionnaireViewModel _viewModel;

        public QuestionnaireViewModelTests()
        {
            _mockNavigationService = new Mock<INavigationService>();
            _mockAlertService = new Mock<IAlertService>();
            _mockQuestionnaireService = new Mock<IQuestionnaireService>();
            _mockSessionService = new Mock<ISessionService>();

            var questionnaireRecordDto = new QuestionnaireRecordDto
            {
                PatientId = 123,
                VisitId = 456,
                Id = 789,
                QuestionnaireType = QuestionnaireType.CGIDoctor,
            };
            _mockSessionService.Setup(s => s.QuestionnaireRecord).Returns(questionnaireRecordDto);

            _mockQuestionnaireService.Setup(x => x.GetQuestionnaireDataAsync(It.IsAny<int>())).ReturnsAsync(
                new QuestionnaireDto
                {
                    BlobPath = "path",
                    Questions = [
                        new QuestionDto
                        {
                            Id = 1,
                            Type = "Radio",
                            Text = "Text",
                            Options = [
                                new OptionDto
                                {
                                    Id = 1,
                                    Text = "Answer",
                                    Value = "1"
                                }
                            ]
                        },
                        new QuestionDto
                        {
                            Id = 2,
                            Type = "Dropdown",
                            Text = "Text",
                            Options = [
                                new OptionDto
                                {
                                    Id = 1,
                                    Text = "Answer",
                                    Value = "1"
                                }
                            ]
                        }
                    ]
                });

            _viewModel = new QuestionnaireViewModel(
            _mockNavigationService.Object,
            _mockAlertService.Object,
            _mockQuestionnaireService.Object,
            _mockSessionService.Object
        );
        }

        [Fact]
        public async Task Constructor_Initializes_Properties_Correctly()
        {
            await _viewModel.LoadQuestions();
            Assert.Equal(0, _viewModel.CurrentIndex);
            Assert.NotNull(_viewModel.CurrentQuestion);
            Assert.Equal(_viewModel.AnswerControls.Count, _viewModel.CurrentQuestion.Options.Count());
            Assert.Equal("Next", _viewModel.NextButtonText);
            Assert.False(_viewModel.IsBackButtonEnabled);
            Assert.False(_viewModel.IsNextButtonEnabled);
        }

        [Fact]
        public async Task NextCommand_Executes_Correctly()
        {
            await _viewModel.LoadQuestions();

            _viewModel.OnNext();

            Assert.Equal(1, _viewModel.CurrentIndex);
            Assert.Equal(_viewModel.CurrentQuestion, _viewModel.QuestionnaireDto.Questions.ElementAt(1));
            Assert.True(_viewModel.IsBackButtonEnabled);
            Assert.False(_viewModel.IsNextButtonEnabled);
        }

        [Fact]
        public async Task BackCommand_Executes_Correctly()
        {
            await _viewModel.LoadQuestions();

            _viewModel.OnNext();
            _viewModel.BackCommand.Execute(null);

            Assert.Equal(0, _viewModel.CurrentIndex);
            Assert.Equal(_viewModel.CurrentQuestion, _viewModel.QuestionnaireDto.Questions.ElementAt(0));
        }

        [Fact]
        public async Task SetAnswer_Updates_SelectedAnswers_And_Enables_NextButton()
        {
            await _viewModel.LoadQuestions();

            var answer = new OptionDto { Id = 1, Text = "Sample Answer", Value = "1" };
            _viewModel.SetAnswer(answer);

            Assert.Equal(answer, _viewModel.SelectedOptions[_viewModel.CurrentIndex]);
            Assert.True(_viewModel.IsNextButtonEnabled);
        }

        [Theory]
        [InlineData(nameof(QuestionnaireViewModel.CurrentQuestion))]
        [InlineData(nameof(QuestionnaireViewModel.NextButtonText))]
        [InlineData(nameof(QuestionnaireViewModel.Progress))]
        [InlineData(nameof(QuestionnaireViewModel.IsBackButtonEnabled))]
        [InlineData(nameof(QuestionnaireViewModel.IsNextButtonEnabled))]
        public async Task Setting_Property_Raises_PropertyChanged(string propertyName)
        {
            await _viewModel.LoadQuestions();

            var eventRaised = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == propertyName)
                {
                    eventRaised = true;
                }
            };

            SetProperty(_viewModel, propertyName);

            Assert.True(eventRaised);
        }

        private async Task SetProperty(QuestionnaireViewModel viewModel, string propertyName)     //test helper function
        {
            switch (propertyName)
            {
                case nameof(QuestionnaireViewModel.CurrentQuestion):
                    viewModel.CurrentQuestion = new QuestionDto();
                    break;
                case nameof(QuestionnaireViewModel.NextButtonText):
                    viewModel.NextButtonText = "Submit";
                    break;
                case nameof(QuestionnaireViewModel.Progress):
                    viewModel.Progress = 50;
                    break;
                case nameof(QuestionnaireViewModel.IsBackButtonEnabled):
                    viewModel.IsBackButtonEnabled = true;
                    break;
                case nameof(QuestionnaireViewModel.IsNextButtonEnabled):
                    viewModel.IsNextButtonEnabled = true;
                    break;
                default:
                    throw new ArgumentException("Invalid property name");
            }
        }
    }
}
