using CommunityToolkit.Mvvm.ComponentModel;
using MySARAssist.Models.Exceptions;
using MySARAssist.Services;
using MySarAssistModels.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySARAssist.ViewModels.CheckInOut
{
    public class CheckInManagementViewModel : ObservableObject
    {
        private readonly PersonnelService _personnelService;
        private bool _isNavigating = false;

        public CheckInManagementViewModel()
        {
            this._personnelService = new PersonnelService();
            SignInCommand = new Command(OnSignInCommand);
            SignOutCommand = new Command(OnSignOutCommand);
            EditTeamMembersCommand = new Command(OnEditTeamMembersCommand);
            ChangeSelectedMemberCommand = new Command(OnChangeSelectedMemberCommand);
            AddMemberCommand = new Command(OnAddMember);
            EditMemberCommand = new Command(OnEditMember);
            SetListVisible();
            
        }



       
        private async void SetListVisible()
        {
            try
            {
                var savedPeople = await _personnelService.GetItems();
                ShowSelectUserButton = savedPeople != null && savedPeople.Any();
                OnPropertyChanged(nameof(ShowSelectUserButton));

            }
            catch (PersonnelNotFoundException ex)
            {
                ShowSelectUserButton = false;
                OnPropertyChanged(nameof(ShowSelectUserButton));
            }


        }
        public Command SignInCommand { get; }
        public Command SignOutCommand { get; }
        public Command EditTeamMembersCommand { get; }
        public Command ChangeSelectedMemberCommand { get; }
        public Command AddMemberCommand { get; set; }
        public Command EditMemberCommand { get; }

        public void OnAppearing()
        {
            OnPropertyChanged(nameof(CurrentMemberName));
            OnPropertyChanged(nameof(AllowSignInAndOut));
            OnPropertyChanged(nameof(ShowCreateNewUser));
            SetListVisible();


        }

        public string CurrentMemberName
        {
            get
            {
                if (App.CurrentPerson != null) { return App.CurrentPerson.Name??"-No member selected-"; }
                else { return "-No member selected-"; }
            }
        }

        public bool AllowSignInAndOut
        {
            get
            {
                return App.CurrentPerson != null;
            }
        }
        public bool ShowCreateNewUser
        {
            get { return !AllowSignInAndOut; }
        }

        public bool ShowSelectUserButton { get; set; }

        private async void OnSignInCommand()
        {
            if (_isNavigating) { return; }
            try
            {
                _isNavigating = true;
                await Shell.Current.GoToAsync(nameof(Views.CheckInOut.CheckInView));
            }
            finally
            {
                _isNavigating = false;
            }
        }

        private async void OnSignOutCommand()
        {
            if (_isNavigating) { return; }
            try
            {
                _isNavigating = true;
                await Shell.Current.GoToAsync(nameof(Views.CheckInOut.CheckOutView));
            }
            finally
            {
                _isNavigating = false;
            }
        }

        private async void OnEditTeamMembersCommand()
        {
            if (_isNavigating) { return; }
            try
            {
                _isNavigating = true;
                if (App.CurrentPerson != null) { await Shell.Current.GoToAsync($"{nameof(Views.CheckInOut.CheckInOutView)}/{nameof(Views.CheckInOut.PersonnelEditView)}?PersonnelID={App.CurrentPerson.ID}"); }
                else { await Shell.Current.GoToAsync($"{nameof(Views.CheckInOut.CheckInOutView)}/{nameof(Views.CheckInOut.PersonnelEditView)}"); }
            }
            finally
            {
                _isNavigating = false;
            }
        }

        private async void OnChangeSelectedMemberCommand()
        {
            if (_isNavigating) { return; }
            try
            {
                _isNavigating = true;
                await Shell.Current.GoToAsync($"{nameof(Views.CheckInOut.CheckInOutView)}/{nameof(Views.CheckInOut.PersonnelListView)}");
            }
            finally
            {
                _isNavigating = false;
            }
        }

        public async void OnAddMember()
        {
            if (_isNavigating) { return; }
            try
            {
                _isNavigating = true;
                await Shell.Current.GoToAsync($"{nameof(Views.CheckInOut.PersonnelEditView)}");
            }
            finally
            {
                _isNavigating = false;
            }
        }

        public async void OnEditMember()
        {
            if (_isNavigating) { return; }
            try
            {
                _isNavigating = true;
                if (App.CurrentPerson != null) { await Shell.Current.GoToAsync($"{nameof(Views.CheckInOut.CheckInOutView)}/{nameof(Views.CheckInOut.PersonnelEditView)}?PersonnelID={App.CurrentPerson.ID}"); }
                else { await Shell.Current.GoToAsync($"{nameof(Views.CheckInOut.CheckInOutView)}/{nameof(Views.CheckInOut.PersonnelEditView)}"); }
            }
            finally
            {
                _isNavigating = false;
            }
        }
    }
}
