using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Storage;
using MySarAssistModels.Assignments;
using MySarAssistModels.Clues;
using MySARAssist.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MySARAssist.ViewModels.DebriefViewModels
{
    public partial class TeamDebriefViewModel : ObservableObject
    {
        private readonly DebriefService _debriefService;
        private CancellationTokenSource? _autoSaveCts;
        private bool _isLoading;

        public AssignmentDebrief Debrief { get; private set; }

        [ObservableProperty]
        private int _assignmentNumber;

        [ObservableProperty]
        private string _teamName = string.Empty;

        [ObservableProperty]
        private DateTime _assignmentStartDate = DateTime.Today;

        [ObservableProperty]
        private TimeSpan _assignmentStartTime = DateTime.Now.TimeOfDay;

        [ObservableProperty]
        private DateTime _assignmentEndDate = DateTime.Today;

        [ObservableProperty]
        private TimeSpan _assignmentEndTime = DateTime.Now.TimeOfDay;

        [ObservableProperty]
        private DateTime _debriefDate = DateTime.Today;

        [ObservableProperty]
        private TimeSpan _debriefTime = DateTime.Now.TimeOfDay;

        [ObservableProperty]
        private string? _whatYourTeamDid;

        [ObservableProperty]
        private bool _tracksProvided;

        [ObservableProperty]
        private bool _deviation;

        [ObservableProperty]
        private string? _environmentFactors;

        [ObservableProperty]
        private string? _searcherFactors;

        [ObservableProperty]
        private string? _gaps;

        [ObservableProperty]
        private string? _hazards;

        [ObservableProperty]
        private string? _weather;

        [ObservableProperty]
        private string? _comments;

        [ObservableProperty]
        private double _debriefPOD;

        [ObservableProperty]
        private double _debriefSpacing;

        [ObservableProperty]
        private double _debriefTravelSpeedPerHour;

        [ObservableProperty]
        private double _debriefRangeOfDetection;

        [ObservableProperty]
        private string? _injuries;

        [ObservableProperty]
        private string? _losses;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DoneButtonText))]
        private bool _isComplete;

        [ObservableProperty]
        private bool _isExported;

        [ObservableProperty]
        private string _lastSaveStatus = "Draft ready";

        [ObservableProperty]
        private bool _isQrModalVisible;

        [ObservableProperty]
        private string _qrCodeValue = string.Empty;

        [ObservableProperty]
        private string _qrCodeTitle = string.Empty;

        public ObservableCollection<ClueItemViewModel> Clues { get; } = new();

        public bool HasClues => Clues.Count > 0;
        public bool HasNoClues => Clues.Count == 0;
        public string DoneButtonText => IsComplete ? "Reopen Debrief for Editing" : "Mark Debrief as Complete";

        public TeamDebriefViewModel()
        {
            _debriefService = new DebriefService();
            Debrief = new AssignmentDebrief();
        }

        public TeamDebriefViewModel(DebriefService debriefService)
        {
            _debriefService = debriefService ?? new DebriefService();
            Debrief = new AssignmentDebrief();
        }

        public async Task LoadDebriefAsync(Guid? debriefId = null)
        {
            _isLoading = true;
            try
            {
                AssignmentDebrief? target = null;
                if (debriefId.HasValue && debriefId.Value != Guid.Empty)
                {
                    target = await _debriefService.GetDebriefAsync(debriefId.Value);
                }
                else
                {
                    target = await _debriefService.GetActiveDebriefAsync();
                }

                if (target == null)
                {
                    target = new AssignmentDebrief
                    {
                        AssignmentStart = DateTime.UtcNow.AddHours(-2),
                        AssignmentEnd = DateTime.UtcNow,
                        DebriefTime = DateTime.UtcNow
                    };
                    await _debriefService.SaveDebriefAsync(target);
                }

                Debrief = target;

                // Bind properties
                AssignmentNumber = Debrief.AssignmentNumber;
                TeamName = Debrief.TeamName;
                AssignmentStartDate = Debrief.AssignmentStart.ToLocalTime().Date;
                AssignmentStartTime = Debrief.AssignmentStart.ToLocalTime().TimeOfDay;
                AssignmentEndDate = Debrief.AssignmentEnd.ToLocalTime().Date;
                AssignmentEndTime = Debrief.AssignmentEnd.ToLocalTime().TimeOfDay;
                DebriefDate = Debrief.DebriefTime.ToLocalTime().Date;
                DebriefTime = Debrief.DebriefTime.ToLocalTime().TimeOfDay;

                WhatYourTeamDid = Debrief.WhatYourTeamDid;
                TracksProvided = Debrief.TracksProvided;
                Deviation = Debrief.Deviation;
                EnvironmentFactors = Debrief.EnvironmentFactors;
                SearcherFactors = Debrief.SearcherFactors;
                Gaps = Debrief.Gaps;
                Hazards = Debrief.Hazards;
                Weather = Debrief.Weather;
                Comments = Debrief.Comments;

                DebriefPOD = Debrief.DebriefPOD;
                DebriefSpacing = Debrief.DebriefSpacing;
                DebriefTravelSpeedPerHour = Debrief.DebriefTravelSpeedPerHour;
                DebriefRangeOfDetection = Debrief.DebriefRangeOfDetection;

                Injuries = Debrief.Injuries;
                Losses = Debrief.Losses;
                IsComplete = Debrief.IsComplete;
                IsExported = Debrief.IsExported;

                // Load clues
                Clues.Clear();
                var dbClues = await _debriefService.GetCluesForAssignmentAsync(Debrief.AssignmentID);
                foreach (var clue in dbClues)
                {
                    Clues.Add(new ClueItemViewModel(clue, OnClueUpdated));
                }
                OnPropertyChanged(nameof(HasClues));
                LastSaveStatus = $"Draft loaded ({DateTime.Now:HH:mm})";
            }
            finally
            {
                _isLoading = false;
            }
        }

        #region Change Handlers & Auto-Save

        partial void OnAssignmentNumberChanged(int value) { if (!_isLoading) { Debrief.AssignmentNumber = value; ScheduleAutoSave(); } }
        partial void OnTeamNameChanged(string value) { if (!_isLoading) { Debrief.TeamName = value; ScheduleAutoSave(); } }

        partial void OnAssignmentStartDateChanged(DateTime value) { if (!_isLoading) { SyncAssignmentStart(); } }
        partial void OnAssignmentStartTimeChanged(TimeSpan value) { if (!_isLoading) { SyncAssignmentStart(); } }

        private void SyncAssignmentStart()
        {
            Debrief.AssignmentStart = (AssignmentStartDate.Date + AssignmentStartTime).ToUniversalTime();
            ScheduleAutoSave();
        }

        partial void OnAssignmentEndDateChanged(DateTime value) { if (!_isLoading) { SyncAssignmentEnd(); } }
        partial void OnAssignmentEndTimeChanged(TimeSpan value) { if (!_isLoading) { SyncAssignmentEnd(); } }

        private void SyncAssignmentEnd()
        {
            Debrief.AssignmentEnd = (AssignmentEndDate.Date + AssignmentEndTime).ToUniversalTime();
            ScheduleAutoSave();
        }

        partial void OnDebriefDateChanged(DateTime value) { if (!_isLoading) { SyncDebriefTime(); } }
        partial void OnDebriefTimeChanged(TimeSpan value) { if (!_isLoading) { SyncDebriefTime(); } }

        private void SyncDebriefTime()
        {
            Debrief.DebriefTime = (DebriefDate.Date + DebriefTime).ToUniversalTime();
            ScheduleAutoSave();
        }

        partial void OnWhatYourTeamDidChanged(string? value) { if (!_isLoading) { Debrief.WhatYourTeamDid = value; ScheduleAutoSave(); } }
        partial void OnTracksProvidedChanged(bool value) { if (!_isLoading) { Debrief.TracksProvided = value; ScheduleAutoSave(); } }
        partial void OnDeviationChanged(bool value) { if (!_isLoading) { Debrief.Deviation = value; ScheduleAutoSave(); } }
        partial void OnEnvironmentFactorsChanged(string? value) { if (!_isLoading) { Debrief.EnvironmentFactors = value; ScheduleAutoSave(); } }
        partial void OnSearcherFactorsChanged(string? value) { if (!_isLoading) { Debrief.SearcherFactors = value; ScheduleAutoSave(); } }
        partial void OnGapsChanged(string? value) { if (!_isLoading) { Debrief.Gaps = value; ScheduleAutoSave(); } }
        partial void OnHazardsChanged(string? value) { if (!_isLoading) { Debrief.Hazards = value; ScheduleAutoSave(); } }
        partial void OnWeatherChanged(string? value) { if (!_isLoading) { Debrief.Weather = value; ScheduleAutoSave(); } }
        partial void OnCommentsChanged(string? value) { if (!_isLoading) { Debrief.Comments = value; ScheduleAutoSave(); } }

        partial void OnDebriefPODChanged(double value) { if (!_isLoading) { Debrief.DebriefPOD = value; ScheduleAutoSave(); } }
        partial void OnDebriefSpacingChanged(double value) { if (!_isLoading) { Debrief.DebriefSpacing = value; ScheduleAutoSave(); } }
        partial void OnDebriefTravelSpeedPerHourChanged(double value) { if (!_isLoading) { Debrief.DebriefTravelSpeedPerHour = value; ScheduleAutoSave(); } }
        partial void OnDebriefRangeOfDetectionChanged(double value) { if (!_isLoading) { Debrief.DebriefRangeOfDetection = value; ScheduleAutoSave(); } }

        partial void OnInjuriesChanged(string? value) { if (!_isLoading) { Debrief.Injuries = value; ScheduleAutoSave(); } }
        partial void OnLossesChanged(string? value) { if (!_isLoading) { Debrief.Losses = value; ScheduleAutoSave(); } }

        private void OnClueUpdated()
        {
            ScheduleAutoSave();
        }

        public void ScheduleAutoSave()
        {
            _autoSaveCts?.Cancel();
            _autoSaveCts = new CancellationTokenSource();
            var token = _autoSaveCts.Token;

            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(500, token);
                    if (token.IsCancellationRequested) return;

                    await SaveToDatabaseAsync();
                }
                catch (TaskCanceledException) { }
            }, token);
        }

        public async Task SaveToDatabaseAsync()
        {
            await _debriefService.SaveDebriefAsync(Debrief);

            foreach (var clueVm in Clues)
            {
                clueVm.Model.AssignmentID = Debrief.AssignmentID;
                clueVm.Model.TaskID = Debrief.TaskID;
                await _debriefService.SaveClueAsync(clueVm.Model);
            }

            LastSaveStatus = $"Saved {DateTime.Now:HH:mm:ss}";
        }

        #endregion

        #region Clues Actions

        [RelayCommand]
        public async Task AddClueAsync()
        {
            var newClue = new Clue
            {
                AssignmentID = Debrief.AssignmentID,
                TaskID = Debrief.TaskID,
                ClueNumber = Clues.Count + 1,
                DateFound = DateTime.UtcNow,
                RecordedBy = TeamName
            };

            await _debriefService.SaveClueAsync(newClue);
            Clues.Add(new ClueItemViewModel(newClue, OnClueUpdated));
            OnPropertyChanged(nameof(HasClues));
            ScheduleAutoSave();
        }

        [RelayCommand]
        public async Task DeleteClueAsync(ClueItemViewModel item)
        {
            if (item == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Clue",
                $"Are you sure you want to delete Clue #{item.ClueNumber}?",
                "Delete", "Cancel");

            if (confirm)
            {
                await _debriefService.DeleteClueAsync(item.Model.ID);
                Clues.Remove(item);

                // Renumber remaining clues
                for (int i = 0; i < Clues.Count; i++)
                {
                    Clues[i].ClueNumber = i + 1;
                    await _debriefService.SaveClueAsync(Clues[i].Model);
                }

                OnPropertyChanged(nameof(HasClues));
                ScheduleAutoSave();
            }
        }

        #endregion

        #region Lifecycle & Status Commands

        [RelayCommand]
        public async Task ToggleDoneAsync()
        {
            IsComplete = !IsComplete;
            Debrief.IsComplete = IsComplete;
            await _debriefService.SaveDebriefAsync(Debrief);

            string message = IsComplete
                ? "Debrief marked as complete. It is ready for export."
                : "Debrief reopened for editing.";
            await Shell.Current.DisplayAlert("Status Updated", message, "OK");
        }

        [RelayCommand]
        public async Task ExportOptionsAsync()
        {
            await SaveToDatabaseAsync();

            var package = await _debriefService.CreateDebriefPackageAsync(Debrief.ID, AppInfo.Current.VersionString);
            if (package == null)
            {
                await Shell.Current.DisplayAlert("Export Error", "Unable to build debrief package.", "OK");
                return;
            }

            bool canFitDirectQr = AssignmentDebriefPackageTools.CanFitInQrCode(package, 2048);

            string optionMysca = "Share .mysca File (Full Package)";
            string optionQrTextOnly = "Show QR Code (Text Only / Exclude Photos)";
            string optionQrFull = "Show QR Code (Full Package)";

            string action;
            if (canFitDirectQr)
            {
                action = await Shell.Current.DisplayActionSheet(
                    "Export Debrief Package", "Cancel", null,
                    optionMysca, optionQrFull);
            }
            else
            {
                action = await Shell.Current.DisplayActionSheet(
                    "Export Debrief Package", "Cancel", null,
                    optionMysca, optionQrTextOnly);
            }

            if (action == optionMysca)
            {
                await ExportMyscaFileAsync(package);
            }
            else if (action == optionQrFull)
            {
                ShowQrModal(AssignmentDebriefPackageTools.ToJson(package, indented: false), $"Debrief: {package.TeamName}");
            }
            else if (action == optionQrTextOnly)
            {
                var stripped = AssignmentDebriefPackageTools.StripPhotos(package);
                ShowQrModal(AssignmentDebriefPackageTools.ToJson(stripped, indented: false), $"Debrief (Text Only): {package.TeamName}");
            }
        }

        private async Task ExportMyscaFileAsync(AssignmentDebriefPackage package)
        {
            try
            {
                string json = AssignmentDebriefPackageTools.ToJson(package, indented: true);
                string filename = $"Debrief_Asg{package.AssignmentNumber}_{DateTime.Now:yyyyMMdd_HHmm}.mysca";
                string tempPath = Path.Combine(FileSystem.CacheDirectory, filename);

                await File.WriteAllTextAsync(tempPath, json);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = $"Debrief: Assignment #{package.AssignmentNumber} ({package.TeamName})",
                    File = new ShareFile(tempPath)
                });

                // Mark exported
                await _debriefService.MarkExportedAsync(Debrief.ID);
                IsExported = true;
                LastSaveStatus = $"Exported {DateTime.Now:HH:mm}";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Export Failed", ex.Message, "OK");
            }
        }

        private void ShowQrModal(string qrData, string title)
        {
            QrCodeValue = qrData;
            QrCodeTitle = title;
            IsQrModalVisible = true;
        }

        [RelayCommand]
        public void CloseQrModal()
        {
            IsQrModalVisible = false;
        }

        [RelayCommand]
        public void ApplyScannedAssignmentJson(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("taskID", out var propTask) && propTask.TryGetGuid(out var taskGuid))
                {
                    Debrief.TaskID = taskGuid;
                }

                if (root.TryGetProperty("assignmentID", out var propAsg) && propAsg.TryGetGuid(out var asgGuid))
                {
                    Debrief.AssignmentID = asgGuid;
                }

                if (root.TryGetProperty("assignmentNumber", out var propNum) && propNum.TryGetInt32(out var asgNum))
                {
                    AssignmentNumber = asgNum;
                }

                if (root.TryGetProperty("teamName", out var propTeam))
                {
                    TeamName = propTeam.GetString() ?? string.Empty;
                }

                ScheduleAutoSave();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to parse assignment QR: {ex.Message}");
            }
        }

        #endregion
    }
}

