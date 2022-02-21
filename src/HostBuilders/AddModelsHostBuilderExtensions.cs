namespace WPFordle.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;

public static class AddModelsHostBuilderExtensions
{
    #region Methods

    public static IHostBuilder AddModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(
            services =>
            {
                services.AddScoped<GameModel>();
            });

        return hostBuilder;
    }

    #endregion
}