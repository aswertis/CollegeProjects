using Microsoft.Maui.Controls;
using EventApp.ViewModels.UserViewModels;

namespace EventApp.Views.User
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage(AccountViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as AccountViewModel)?.LoadUserCommand.Execute(null);
        }
    }
}