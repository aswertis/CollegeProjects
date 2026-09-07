using EventApp.ViewModels.Auth;
using Microsoft.Maui.Controls;

namespace EventApp.Views.Auth
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}