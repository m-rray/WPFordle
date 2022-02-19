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
                services.AddSingleton<MainWindowViewModel>();
            });

        return hostBuilder;
    }

    #endregion
}