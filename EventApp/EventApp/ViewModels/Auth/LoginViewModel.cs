using EventApp.Services;
using EventApp.ViewModels.Shared;
using EventApp.Views.Auth;
using EventApp.Views.User;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventApp.ViewModels.Auth
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _username;
        private string _password;
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

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand GuestCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await Login());
            RegisterCommand = new Command(async () => await Register());
            GuestCommand = new Command(async () => await GuestLogin());
            ForgotPasswordCommand = new Command(async () => await ForgotPassword());
        }

        private async Task Login()
        {
            if (IsBusy) return;
            IsBusy = true;

            var success = await _authService.Login(Username, Password);
            if (success)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Неверный логин или пароль", "OK");
            }

            IsBusy = false;
        }

        private async Task GuestLogin()
        {
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//EventsPage");
        }

        private async Task Register()
        {
            var vm = App.ServiceProvider.GetRequiredService<RegisterViewModel>();
            var page = new RegisterPage(vm);

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async Task ForgotPassword()
        {
            var vm = App.ServiceProvider.GetRequiredService<PasswordResetViewModel>();
            var page = new PasswordResetPage(vm);

            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}