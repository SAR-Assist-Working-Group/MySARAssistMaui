using Microsoft.Maui.Controls;
using MySARAssist.ViewModels.DebriefViewModels;
using System;

namespace MySARAssist.Views.DebriefViews
{
    [QueryProperty(nameof(DebriefId), "DebriefId")]
    public partial class TeamDebriefPage : ContentPage
    {
        private readonly TeamDebriefViewModel _viewModel;
        private string? _debriefIdString;

        public string? DebriefId
        {
            get => _debriefIdString;
            set
            {
                _debriefIdString = value;
                if (Guid.TryParse(value, out Guid id))
                {
                    _ = _viewModel.LoadDebriefAsync(id);
                }
            }
        }

        public TeamDebriefPage()
        {
            InitializeComponent();
            _viewModel = new TeamDebriefViewModel();
            BindingContext = _viewModel;
        }

        public TeamDebriefPage(TeamDebriefViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrEmpty(_debriefIdString))
            {
                await _viewModel.LoadDebriefAsync();
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.SaveToDatabaseAsync();
        }

        private async void OnScanQrClicked(object sender, EventArgs e)
        {
            ScanAssignmentSheetPage.OnBarcodeScanned = (scannedJson) =>
            {
                _viewModel.ApplyScannedAssignmentJson(scannedJson);
            };

            await Shell.Current.GoToAsync(nameof(ScanAssignmentSheetPage));
        }

        private async void OnHistoryClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DebriefHistoryPage));
        }
    }
}

