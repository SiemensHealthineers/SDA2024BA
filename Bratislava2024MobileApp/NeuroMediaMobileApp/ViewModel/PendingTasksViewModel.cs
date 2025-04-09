using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Maui.Media;

using NeuroMediaMobileApp.Helpers;
using NeuroMediaMobileApp.Models.DTOs;
using NeuroMediaMobileApp.Models.Enums;
using NeuroMediaMobileApp.Services;
using NeuroMediaMobileApp.Services.Interfaces;
using NeuroMediaMobileApp.View;

namespace NeuroMediaMobileApp.ViewModel
{
    public partial class PendingTasksViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;
        private readonly IVisitService _visitService;
        private readonly IVideoService _videoService;
        private readonly IAlertService _alertService;
        private GetActualVisitDto _actualVisit;
        public ObservableCollection<string> Tasks { get; }

        [ObservableProperty]
        private string videoPath;
        public PendingTasksViewModel(INavigationService navigationService, ISessionService sessionService, IVisitService visitService, IVideoService videoService, IAlertService alertService)
        {
            _navigationService = navigationService;
            _sessionService = sessionService;
            _visitService = visitService;
            _videoService = videoService;
            _alertService = alertService;
            Tasks = new ObservableCollection<string>();
        }


        public async Task LoadPendingTasksAsync(int patientId)
        {
            try
            {
                _actualVisit = await _visitService.GetActualVisitAsync(patientId);
                Tasks.Clear();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }
            catch
            {
                return;
            }

            foreach (var questionnaire in _actualVisit.Questionnaires)
            {
                if (questionnaire == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(questionnaire.BlobPath))
                {
                    Tasks.Add(questionnaire.QuestionnaireType.ToString());
                }
            }
            foreach (var video in _actualVisit.Videos)
            {
                if (video == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(video.BlobPath))
                {
                    Tasks.Add(video.VideoType.ToString());
                }
            }
            OnPropertyChanged(nameof(Tasks));
        }

        [RelayCommand]
        public async Task NavigateToPendingTask(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                try
                {
                    if (Enum.TryParse(source, out QuestionnaireType questionnaireType))
                    {
                        var questionnaireDto = _actualVisit.Questionnaires
                                         .FirstOrDefault(q => q.QuestionnaireType == questionnaireType);
                        _sessionService.QuestionnaireRecord = new QuestionnaireRecordDto
                        {
                            PatientId = questionnaireDto.PatientId,
                            VisitId = questionnaireDto.VisitId,
                            Id = questionnaireDto.Id,
                            QuestionnaireType = questionnaireType,
                        };
                        await _navigationService.NavigateToAsync(nameof(QuestionnairePage));
                        return;
                    }
                    else if (Enum.TryParse(source, out VideoType videoType))
                    {
                        var videoInfoDto = _actualVisit.Videos
                                         .FirstOrDefault(q => q.VideoType == videoType);
                        var instructions = "";

                        switch (videoType)
                        {
                            case VideoType.Front:
                                instructions = "Pacient by mal mať oblečený odev bez límca. Pokiaľ má dlhé vlasy, jej nutné si ich vypnúť/dať do copu. Dbajte na vyhovujúce svetelné podmienky.\n\nPokyny:\n1. Zatvorte si oči a nechajte hlavu pohybovať do najpohodlnejšej polohy po dobu 30 sekúnd.\n\n2. Držte hlavu rovno bez pohnutia po dobu 30 sekúnd.";
                                break;
                            case VideoType.Profile:
                                instructions = "Pacient by mal mať oblečený odev bez límca. Pokiaľ má dlhé vlasy, jej nutné si ich vypnúť/dať do copu. Dbajte na vyhovujúce svetelné podmienky.\n\nPokyny:\n1. Pozerajte sa 10 sekúnd pred seba. Hlavu neotáčajte za kamerou. Na strane nezáleží.\n\n2. Hýbte hlavou doprava a doľava trikrát za sebou doprava aj doľava.\n\n3. Hýbte hlavou hore dole trikrát za sebou hore aj dole.";
                                break;
                            case VideoType.Walk:
                                instructions = "Pacient by mal mať oblečený odev bez límca. Pokiaľ má dlhé vlasy, jej nutné si ich vypnúť/dať do copu. Dbajte na vyhovujúce svetelné podmienky.\n\nPokyny:\n1. Prejdite sa pohodlnou rýchlosťou 10 krokov tam aj naspäť. Pokiaľ vám priestor nedovoľuje spraviť 10 krokov, spravte ich čo najviac, tak aby boli zachytené nohy.";
                                break;
                        }
                        await _alertService.DisplayAlertAsync(
                                    "Inštrukcie",
                                    instructions,
                                    "Ok");

                        var pendingTaskDto = new PendingTaskDto
                        {
                            PendingTaskType = PendingTaskType.Video,
                            Name = source,
                            PatientId = videoInfoDto!.PatientId,
                            VisitId = videoInfoDto.VisitId,
                            Id = videoInfoDto.Id,
                            QuestionnaireType = null,
                            VideoType = videoType
                        };

                        await RecordVideo(pendingTaskDto);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await _alertService.DisplayAlertAsync(
                        "Error",
                        $"Exception: {ex.Message}",
                        "OK");
                }
            }
            else
            {
                await _navigationService.DisplayNavigationErrorAlertAsync("Error", "Something went wrong! Please, try again.", "OK");
            }
        }

        private async Task RecordVideo(PendingTaskDto pendingTaskDto)
        {
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (cameraStatus != PermissionStatus.Granted)
            {
                cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();

                if (cameraStatus != PermissionStatus.Granted)
                {
                    await _alertService.DisplayAlertAsync(
                        "Camera Permission",
                        "The camera permission is required to record video. Please enable camera access in your device settings.",
                        "OK");
                    return;
                }
            }

            if (MediaPicker.Default.IsCaptureSupported)
            {
                var video = await MediaPicker.Default.CaptureVideoAsync();

                if (video != null)
                {
                    VideoPath = Path.Combine(FileSystem.CacheDirectory, video.FileName);

                    using var sourceStream = await video.OpenReadAsync();
                    using var localFileStream = File.OpenWrite(VideoPath);

                    await sourceStream.CopyToAsync(localFileStream);

                    var fileName = $"{DateTime.UtcNow.ToFileTime().ToString(CultureInfo.InvariantCulture)}.{VideoHelper.VideoFileExt}";
                    var blobPath = VideoHelper.GetVideoFilePath((int) pendingTaskDto.PatientId!, (int) pendingTaskDto.VisitId!, (int) pendingTaskDto.Id!, (VideoType) pendingTaskDto.VideoType!, fileName);
                    var response = await _videoService.UploadVideoFileAsync(blobPath, sourceStream, fileName);

                    if (response.IsSuccessStatusCode)
                    {
                        await _alertService.DisplayAlertAsync("Success", "Video captured and saved successfully.", "OK");
                        Tasks.Remove(pendingTaskDto.Name);
                    }
                    else
                    {
                        await _alertService.DisplayAlertAsync("Error", "Video upload failed.", "OK");
                    }
                }
                else
                {
                    await _alertService.DisplayAlertAsync("Error",
                        "Video capture was canceled or failed.", "OK");
                }
            }
            else
            {
                await _alertService.DisplayAlertAsync("Error",
                    "Video capture is not supported on this device.", "OK");
            }
        }

    }
}
