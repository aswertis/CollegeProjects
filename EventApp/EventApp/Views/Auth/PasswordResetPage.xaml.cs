using EventApp.ViewModels.Auth;
using Microsoft.Maui.Controls;

namespace EventApp.Views.Auth
{
    public partial class PasswordResetPage : ContentPage
    {
        public PasswordResetPage(PasswordResetViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}