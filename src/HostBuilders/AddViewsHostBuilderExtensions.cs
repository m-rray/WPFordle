namespace WPFordle.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Views;

public static class AddViewsHostBuilderExtensions
{
    #region Methods

    public static IHostBuilder AddViews(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<MainWindow>();
        });

        return hostBuilder;
    }

    #endregion
}