using EventApp.ViewModels.UserViewModels;
using Microsoft.Maui.Controls;

namespace EventApp.Views.User
{
    public partial class EventsPage : ContentPage
    {
        public EventsPage(EventsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as EventsViewModel)?.LoadEventsCommand.Execute(null);
        }
    }
}