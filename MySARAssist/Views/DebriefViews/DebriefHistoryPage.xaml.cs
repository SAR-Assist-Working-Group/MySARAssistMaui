using Microsoft.Maui.Controls;
using MySARAssist.ViewModels.DebriefViewModels;

namespace MySARAssist.Views.DebriefViews
{
    public partial class DebriefHistoryPage : ContentPage
    {
        private readonly DebriefHistoryViewModel _viewModel;

        public DebriefHistoryPage()
        {
            InitializeComponent();
            _viewModel = new DebriefHistoryViewModel();
            BindingContext = _viewModel;
        }

        public DebriefHistoryPage(DebriefHistoryViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadDebriefsAsync();
        }
    }
}

