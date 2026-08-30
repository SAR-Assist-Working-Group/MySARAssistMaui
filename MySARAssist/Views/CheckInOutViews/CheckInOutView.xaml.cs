using MetroLog;
using MetroLog.Maui;
using Microsoft.Extensions.Logging;

namespace MySARAssist.Views.CheckInOut;


public partial class CheckInOutView : ContentPage
{
	private readonly ILogger<MainPage> logger;
	private bool _isNavigating = false;

	public CheckInOutView(ILogger<MainPage> logger)
	{
		InitializeComponent();
		this.logger = logger;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		LogController.ResumeShakeIfNeeded();
		if(this.BindingContext != null)
		{
			ViewModels.CheckInOut.CheckInManagementViewModel vm = (ViewModels.CheckInOut.CheckInManagementViewModel)this.BindingContext;
			vm.OnAppearing();

		}
	}

	private async void btnAddUser_Clicked(object sender, EventArgs e)
	{
		if (_isNavigating) { return; }
		try
		{
			_isNavigating = true;
			//await Navigation.PushAsync(new Views.CheckInOut.PersonnelEditView());
			await Shell.Current.GoToAsync("" + nameof(CheckInOutView) + "/" + nameof(PersonnelEditView));
		}
		finally
		{
			_isNavigating = false;
		}
	}

	private async  void btnChangeSelectedMember_Clicked(object sender, EventArgs e)
	{
		if (_isNavigating) { return; }
		try
		{
			_isNavigating = true;
			await Shell.Current.GoToAsync("" + nameof(CheckInOutView) + "/" + nameof(PersonnelListView));
		}
		finally
		{
			_isNavigating = false;
		}
	}

	private async void btnEditMember_Clicked(object sender, EventArgs e)
	{
		if (_isNavigating) { return; }
		try
		{
			_isNavigating = true;
			//await Navigation.PushAsync(new Views.CheckInOut.PersonnelEditView());
			if (App.CurrentPerson == null)
			{
				await Shell.Current.GoToAsync("" + nameof(CheckInOutView) + "/" + nameof(PersonnelEditView));
			}
			else
			{
				await Shell.Current.GoToAsync("" + nameof(CheckInOutView) + "/" + nameof(PersonnelEditView) + $"PersonnelID={App.CurrentPerson.ID.ToString()}");

				//await Shell.Current.GoToAsync($"CheckInOut/EditPersonnel?PersonnelID={App.CurrentPerson.ID.ToString()}");
			}
		}
		finally
		{
			_isNavigating = false;
		}
	}

	private async void btnSelectUser_Clicked(object sender, EventArgs e)
	{
		if (_isNavigating) { return; }
		try
		{
			_isNavigating = true;
			await Shell.Current.GoToAsync("" + nameof(CheckInOutView) + "/" + nameof(PersonnelListView));
		}
		finally
		{
			_isNavigating = false;
		}
	}
}