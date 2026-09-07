using EventApp.ViewModels.Auth;
using Microsoft.Maui.Controls;

namespace EventApp.Views.Auth
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}