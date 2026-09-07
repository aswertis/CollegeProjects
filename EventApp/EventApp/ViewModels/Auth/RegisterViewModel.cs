using EventApp.Models;
using EventApp.Services;
using EventApp.ViewModels.Shared;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventApp.ViewModels.Auth
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _username;
        private string _password;
        private string _email;
        private bool _isBusy;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }

        public RegisterViewModel(AuthService authService)
        {
            _authService = authService;
            RegisterCommand = new Command(async () => await Register());
            BackCommand = new Command(async () => await Back());
        }

        private async Task Register()
        {
            if (IsBusy) return;
            IsBusy = true;

            var user = new User
            {
                Username = Username,
                Password = Password,
                Mail = Email,
                IsAdmin = false
            };

            var success = await _authService.Register(user);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Успех", "Регистрация прошла успешно", "OK");
                await Shell.Current.GoToAsync("//Login");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось зарегистрироваться", "OK");
            }

            IsBusy = false;
        }

        private async Task Back()
        {
            await Shell.Current.GoToAsync("//Login");
        }
    }
}