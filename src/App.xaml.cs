namespace WPFordle;

using HostBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Views;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Fields

    private readonly IHost _host;

    #endregion

    #region Constructors

    public App()
    {
        this._host = Host.CreateDefaultBuilder()
            .AddViewModels()
            .AddViews()
            .Build();
    }

    #endregion

    #region Methods

    protected override void OnStartup(StartupEventArgs e)
    {
        this._host.Start();

        this.MainWindow = this._host.Services.GetRequiredService<MainWindow>();
        this.MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        this._host.Dispose();

        base.OnExit(e);
    }

    #endregion
}