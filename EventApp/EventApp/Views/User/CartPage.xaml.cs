using EventApp.ViewModels.UserViewModels;
using Microsoft.Maui.Controls;

namespace EventApp.Views.User
{
    public partial class CartPage : ContentPage
    {
        public CartPage(CartViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as CartViewModel)?.LoadCartCommand.Execute(null);
        }
    }
}