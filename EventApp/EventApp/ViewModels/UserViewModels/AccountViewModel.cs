using EventApp.Models;
using EventApp.Services;
using EventApp.ViewModels.Shared;
using EventApp.Views.User;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventApp.ViewModels.UserViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly IDatabaseService _databaseService;
        private Models.User _currentUser;
        private string _newPassword;
        private bool _isBusy;

        public Models.User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand LoadUserCommand { get; }
        public ICommand UpdatePasswordCommand { get; }
        public ICommand LogoutCommand { get; }

        public AccountViewModel(AuthService authService, IDatabaseService databaseService)
        {
            _authService = authService;
            _databaseService = databaseService;
            LoadUserCommand = new Command(async () => await LoadUser());
            UpdatePasswordCommand = new Command(async () => await UpdatePassword());
            LogoutCommand = new Command(async () => await Logout());
        }

        private async Task LoadUser()
        {
            if (IsBusy) return;
            IsBusy = true;

            CurrentUser = _authService.CurrentUser;

            IsBusy = false;
        }

        private async Task UpdatePassword()
        {
            if (IsBusy) return;
            IsBusy = true;

            var success = await _databaseService.UpdatePassword(CurrentUser.UserId, NewPassword);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Успех", "Пароль изменен", "OK");
                NewPassword = string.Empty;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось изменить пароль", "OK");
            }

            IsBusy = false;
        }

        private async Task Logout()
        {
            _authService.Logout();
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//Login");
        }
    }
}