using EventApp.ViewModels.Auth;
using EventApp.Views.Auth;
using EventApp.Views.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace EventApp
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            InitializeComponent();

            var host = MauiProgram.CreateMauiApp();
            ServiceProvider = host.Services;

            // Проверка, авторизован ли пользователь
            if (IsUserLoggedIn())
            {
                // Если авторизован, отображаем AppShell
                MainPage = new AppShell();
            }
            else
            {
                // Если не авторизован, отображаем страницу авторизации
                MainPage = new NavigationPage(new LoginPage(
                    ServiceProvider.GetRequiredService<LoginViewModel>()));
            }


        }

        // Проверка, авторизован ли пользователь
        private bool IsUserLoggedIn()
        {
            return Preferences.Get("IsUserLoggedIn", false);
        }
    }
}