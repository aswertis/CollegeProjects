using EventApp.Services;
using EventApp.ViewModels.Auth;
using EventApp.ViewModels.UserViewModels;
using EventApp.ViewModels.Shared;
using EventApp.Views.Auth;
using EventApp.Views.User;
using Microsoft.Extensions.Logging;

namespace EventApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Регистрация ViewModels
            builder.Services.AddTransient<BaseViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<PasswordResetViewModel>();

            builder.Services.AddTransient<CartViewModel>();
            builder.Services.AddTransient<EventsViewModel>();
            builder.Services.AddTransient<AccountViewModel>();

            // Регистрация страниц
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<PasswordResetPage>();

            builder.Services.AddTransient<CartPage>();

            // Регистрация сервисов
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<AuthService>();




#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}