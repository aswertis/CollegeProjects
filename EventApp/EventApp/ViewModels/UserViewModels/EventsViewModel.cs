using EventApp.Models;
using EventApp.Services;
using EventApp.ViewModels.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventApp.ViewModels.UserViewModels
{
    public class EventsViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly AuthService _authService;

        private List<Event> _events;
        private bool _isBusy;

        public List<Event> Events
        {
            get => _events;
            set => SetProperty(ref _events, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand LoadEventsCommand { get; }
        public ICommand AddToCartCommand { get; }

        public EventsViewModel(IDatabaseService databaseService, AuthService authService)
        {
            _databaseService = databaseService;
            _authService = authService;

            LoadEventsCommand = new Command(async () => await LoadEvents());
            AddToCartCommand = new Command<Event>(async (e) => await AddToCart(e));
        }

        private async Task LoadEvents()
        {
            if (IsBusy) return;
            IsBusy = true;

            Events = await _databaseService.GetAllEvents();
            OnPropertyChanged(nameof(Events));

            IsBusy = false;
        }

        private async Task AddToCart(Event @event)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (_authService.CurrentUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, авторизуйтесь, чтобы добавить в корзину", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
                IsBusy = false;
                return;
            }
            var order = new Order
            {
                UserId = _authService.CurrentUser.UserId,
                EventId = @event.EventId,
                TotalAmount = @event.Price,
                OrderDate = DateTime.Now
            };

            var result = await _databaseService.AddOrderToCart(order);


            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Успех", "Мероприятие добавлено в корзину", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось добавить в корзину", "OK");
            }

            IsBusy = false;
        }
    }
}