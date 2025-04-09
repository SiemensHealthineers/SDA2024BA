using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using NeuroMediaMobileApp.Models.DTOs;
using NeuroMediaMobileApp.View;
using NeuroMediaMobileApp.Services.Interfaces;

using CommunityToolkit.Mvvm.Input;
using NeuroMediaMobileApp.Helpers;


namespace NeuroMediaMobileApp.ViewModel
{
    public partial class QuestionnaireViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IAlertService _alertService;
        private readonly IQuestionnaireService _questionnaireService;
        private readonly ISessionService _sessionService;
        private QuestionDto _currentQuestion = new QuestionDto();
        public QuestionDto CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        private bool _isBackButtonEnabled;
        public bool IsBackButtonEnabled
        {
            get => _isBackButtonEnabled;
            set
            {
                _isBackButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isNextButtonEnabled;
        public bool IsNextButtonEnabled
        {
            get => _isNextButtonEnabled;
            set
            {
                _isNextButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private string _nextButtonText;
        public string NextButtonText
        {
            get => _nextButtonText;
            set
            {
                _nextButtonText = value;
                OnPropertyChanged();
            }
        }

        private int _currentIndex;
        public int CurrentIndex => _currentIndex;

        private QuestionnaireDto _questionnaireDto;
        public QuestionnaireDto QuestionnaireDto => _questionnaireDto;

        private RadioButton? _checkedRadioButton;
        private List<OptionDto> _selectedOptions = [];
        public IReadOnlyList<OptionDto> SelectedOptions => _selectedOptions.AsReadOnly();
        public ObservableCollection<AnswerDto> Results { get; set; } = [];
        public ObservableCollection<Microsoft.Maui.Controls.View> AnswerControls { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public int CurrentIndex1 { get => _currentIndex; set => _currentIndex = value; }

        public QuestionnaireViewModel(INavigationService navigationService, IAlertService alertService, IQuestionnaireService questionnaireService, ISessionService sessionService)
        {
            _navigationService = navigationService;
            _alertService = alertService;
            _questionnaireService = questionnaireService;
            _sessionService = sessionService;
            AnswerControls = new ObservableCollection<Microsoft.Maui.Controls.View>();
            BackCommand = new Command(OnBack);
            NextCommand = new Command(OnNext);
            _nextButtonText = "Next";
            _currentIndex = 0;
        }

        [RelayCommand]
        public async Task LoadQuestions()
        {
            _questionnaireDto = await _questionnaireService.GetQuestionnaireDataAsync((int) _sessionService.QuestionnaireRecord.QuestionnaireType);
            _selectedOptions = new List<OptionDto>(new OptionDto[_questionnaireDto.Questions.Count()]);
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            CurrentQuestion = _questionnaireDto.Questions.ElementAt(_currentIndex);
            Progress = (double) (_currentIndex + 1) / _questionnaireDto.Questions.Count();
            IsBackButtonEnabled = _currentIndex > 0;
            IsNextButtonEnabled = !(_selectedOptions[_currentIndex] is null);
            AnswerControls.Clear();
            NextButtonText = _currentIndex == _questionnaireDto.Questions.Count() - 1 ? "Submit" : "Next";
            var selectedOptionValue = _selectedOptions[_currentIndex]?.Value;

            if (CurrentQuestion.Type == "Radio")
            {
                _checkedRadioButton = null;
                foreach (var option in CurrentQuestion.Options)
                {
                    var radioButton = new RadioButton { Content = option.Text, Value = option.Value };
                    if (selectedOptionValue == option.Value)
                    {
                        radioButton.IsChecked = true;
                        _checkedRadioButton = radioButton;
                        SetAnswer(option);
                    }

                    radioButton.CheckedChanged += (s, e) =>
                    {
                        if (e.Value)
                        {
                            if (_checkedRadioButton != null && _checkedRadioButton != radioButton)
                            {
                                _checkedRadioButton.IsChecked = false;
                            }

                            _checkedRadioButton = radioButton;
                            SetAnswer(option);
                        }
                    };
                    AnswerControls.Add(radioButton);
                }
            }
            else if (CurrentQuestion.Type == "TextInput")
            {
                var entry = new Entry
                {
                    Placeholder = "Sem napíšte Vašu odpoveď...",
                    Text = selectedOptionValue
                };

                SetAnswer(new OptionDto { Id = 0, Text = entry.Text, Value = entry.Text });
                entry.TextChanged += (s, e) => SetAnswer(new OptionDto { Id = 0, Text = e.NewTextValue, Value = e.NewTextValue });
                AnswerControls.Add(entry);
            }
            else if (CurrentQuestion.Type == "DropDown")
            {
                var picker = new Picker
                {
                    Title = "Vyberte si možnosť",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                foreach (var option in CurrentQuestion.Options)
                {
                    picker.Items.Add(option.Text);
                }

                if (!string.IsNullOrEmpty(selectedOptionValue))
                {
                    var selectedOption = CurrentQuestion.Options.FirstOrDefault(a => a.Value == selectedOptionValue);
                    picker.SelectedIndex = selectedOption.Id;
                }

                picker.SelectedIndexChanged += (s, e) =>
                {
                    if (picker.SelectedIndex != -1)
                    {
                        var selectedOption = CurrentQuestion.Options.ElementAt(picker.SelectedIndex);
                        SetAnswer(selectedOption);
                    }
                };

                AnswerControls.Add(picker);
            }
            IsNextButtonEnabled = !string.IsNullOrEmpty(selectedOptionValue);

        }

        public void SetAnswer(OptionDto answer)
        {
            _selectedOptions[_currentIndex] = answer;
            IsNextButtonEnabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnBack()
        {
            if (_currentIndex > 0)
            {
                SaveCurrentAnswer();
                _currentIndex--;
                ShowQuestion();
            }
        }

        public void OnNext()
        {
            SaveCurrentAnswer();
            if (_currentIndex < _questionnaireDto.Questions.Count() - 1)
            {
                _currentIndex++;
                ShowQuestion();
            }
            else
            {
                SaveResults();
            }
        }

        private async Task SaveResults()
        {
            var resultsDto = new AnswersDto { Answers = Results };
            var blobPath = QuestionnaireHelper.GetResultPath(
                _sessionService.QuestionnaireRecord.PatientId,
                _sessionService.QuestionnaireRecord.VisitId,
                _sessionService.QuestionnaireRecord.Id,
                _sessionService.QuestionnaireRecord.QuestionnaireType.Value,
                $"{DateTime.Now.ToFileTime()}.json");

            var response = await _questionnaireService.SendQuestionnaireResults(blobPath, resultsDto);

            if (response.IsSuccessStatusCode)
            {
                await _alertService.DisplayAlertAsync("Thank You", "Thank you for completing the survey!", "OK");
            }
            else
            {
                await _alertService.DisplayAlertAsync("error", $"Error: {response.StatusCode}", "OK");
                return;
            }

            var navigationStack = Shell.Current.Navigation.NavigationStack.ToList();
            foreach (var page in navigationStack)
            {
                if (page is QuestionnairePage)
                {
                    Shell.Current.Navigation.RemovePage(page);
                }
            }
            await _navigationService.NavigateToAsync(nameof(PatientProfilePage));
        }

        public void SaveCurrentAnswer()
        {
            if (_selectedOptions[_currentIndex] is null)
            {
                return;
            }
            if (_currentQuestion.Type is "Radio" or "DropDown")
            {
                UpdateOrAddResult(_currentQuestion.Id, _selectedOptions[_currentIndex].Id, _selectedOptions[_currentIndex].Value);
            }
            else if (_currentQuestion.Type == "TextInput")
            {
                UpdateOrAddResult(_currentQuestion.Id, 0, _selectedOptions[_currentIndex].Value);
            }
        }

        private void UpdateOrAddResult(int questionId, int optionId, string resultValue)
        {
            var existingResult = Results.FirstOrDefault(r => r.QuestionId == questionId);
            if (existingResult != null)
            {
                existingResult.OptionId = optionId;
                existingResult.ResultValue = resultValue;
            }
            else
            {
                Results.Add(new AnswerDto
                {
                    QuestionId = questionId,
                    OptionId = optionId,
                    ResultValue = resultValue
                });
            }
        }
    }
}
