using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySarAssistModels.Assignments;
using MySARAssist.Services;
using MySARAssist.Views.DebriefViews;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MySARAssist.ViewModels.DebriefViewModels
{
    public partial class DebriefHistoryViewModel : ObservableObject
    {
        private readonly DebriefService _debriefService;

        public ObservableCollection<DebriefHistoryItemViewModel> Debriefs { get; } = new();

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _hasNoDebriefs;

        public DebriefHistoryViewModel()
        {
            _debriefService = new DebriefService();
        }

        public DebriefHistoryViewModel(DebriefService debriefService)
        {
            _debriefService = debriefService ?? new DebriefService();
        }

        [RelayCommand]
        public async Task LoadDebriefsAsync()
        {
            IsLoading = true;
            try
            {
                Debriefs.Clear();
                var items = await _debriefService.GetAllDebriefsAsync();
                foreach (var item in items)
                {
                    Debriefs.Add(new DebriefHistoryItemViewModel(item));
                }
                HasNoDebriefs = Debriefs.Count == 0;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task OpenDebriefAsync(DebriefHistoryItemViewModel item)
        {
            if (item == null) return;
            await Shell.Current.GoToAsync($"{nameof(TeamDebriefPage)}?DebriefId={item.ID}");
        }

        [RelayCommand]
        public async Task CreateNewDebriefAsync()
        {
            var newDebrief = new AssignmentDebrief
            {
                AssignmentStart = DateTime.UtcNow.AddHours(-2),
                AssignmentEnd = DateTime.UtcNow,
                DebriefTime = DateTime.UtcNow
            };

            await _debriefService.SaveDebriefAsync(newDebrief);
            await Shell.Current.GoToAsync($"{nameof(TeamDebriefPage)}?DebriefId={newDebrief.ID}");
        }

        [RelayCommand]
        public async Task DeleteDebriefAsync(DebriefHistoryItemViewModel item)
        {
            if (item == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Debrief",
                $"Are you sure you want to delete {item.Title}?",
                "Delete", "Cancel");

            if (confirm)
            {
                await _debriefService.DeleteDebriefAsync(item.ID);
                Debriefs.Remove(item);
                HasNoDebriefs = Debriefs.Count == 0;
            }
        }
    }
}

