using EventApp.DTO;
using EventApp.Models;
using EventApp.Services;
using EventApp.ViewModels.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;


namespace EventApp.ViewModels.UserViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly AuthService _authService;
        private List<CartItemDisplay> _cartItems;
        private bool _isBusy;

        public List<CartItemDisplay> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public ICommand LoadCartCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand RemoveFromCartCommand { get; }


        public CartViewModel(IDatabaseService databaseService, AuthService authService)
        {
            _databaseService = databaseService;
            _authService = authService;
            LoadCartCommand = new Command(async () => await LoadCart());
            CheckoutCommand = new Command(async () => await Checkout());
            RemoveFromCartCommand = new Command<CartItemDisplay>(async (item) => await RemoveFromCart(item));
        }

        private async Task LoadCart()
        {
            if (IsBusy) return;
            IsBusy = true;

            if (_authService.CurrentUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Вы не авторизованы", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
                IsBusy = false;
                return;
            }

            var orders = await _databaseService.GetOrdersForUser(_authService.CurrentUser.UserId);
            var result = new List<CartItemDisplay>();

            decimal total = 0;

            foreach (var order in orders)
            {
                var ev = await _databaseService.GetEventById(order.EventId);
                result.Add(new CartItemDisplay
                {
                    OrderId = order.OrderId,
                    EventTitle = ev.Title,
                    EventDate = ev.Date,
                    Location = ev.Location,
                    TotalAmount = order.TotalAmount
                });

                total += order.TotalAmount;
            }

            CartItems = result;
            TotalPrice = total;
            IsBusy = false;
        }

        private async Task Checkout()
        {
            if (IsBusy) return;
            IsBusy = true;
            await Application.Current.MainPage.DisplayAlert("Успех", "Заказ оформлен", "OK");

            IsBusy = false;
        }

        private async Task RemoveFromCart(CartItemDisplay item)
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var resultOrder = await _databaseService.DeleteOrderId(item.OrderId);
                if (!resultOrder)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось удалить заказ из корзины", "OK");
                    return;
                }
                CartItems.Remove(item);
                OnPropertyChanged(nameof(CartItems));

                TotalPrice -= item.TotalAmount;
                OnPropertyChanged(nameof(TotalPrice));

                await Application.Current.MainPage.DisplayAlert("Успех", "Товар удален из корзины", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}