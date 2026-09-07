using EventApp.Services;
using EventApp.ViewModels.Shared;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventApp.ViewModels.Auth
{
    public class PasswordResetViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _email;
        private bool _isBusy;

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

        public ICommand ResetPasswordCommand { get; }
        public ICommand BackCommand { get; }

        public PasswordResetViewModel(AuthService authService)
        {
            _authService = authService;
            ResetPasswordCommand = new Command(async () => await ResetPassword());
            BackCommand = new Command(async () => await Back());
        }

        private async Task ResetPassword()
        {
            if (IsBusy) return;
            IsBusy = true;

            bool success = await _authService.ResetPassword(Email);

            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Успех", "Инструкции отправлены на email", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось отправить запрос", "OK");
            }

            IsBusy = false;
        }

        private async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}