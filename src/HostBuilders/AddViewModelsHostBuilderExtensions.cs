namespace WPFordle.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ViewModels;

public static class AddViewModelsHostBuilderExtensions
{
    #region Methods

    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(
            services =>
            {
                services.AddScoped<GameViewModel>();
                services.AddScoped<KeyboardViewModel>();
                services.AddScoped<SettingsViewModel>();
                services.AddScoped<NotificationsViewModel>();
            });

        return hostBuilder;
    }

    #endregion
}