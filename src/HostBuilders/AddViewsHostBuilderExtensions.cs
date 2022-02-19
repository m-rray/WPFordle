namespace WPFordle.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Views;

public static class AddViewsHostBuilderExtensions
{
    #region Methods

    public static IHostBuilder AddViews(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(
            services =>
            {
                services.AddSingleton<MainWindow>();
            });

        return hostBuilder;
    }

    #endregion
}