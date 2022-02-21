namespace WPFordle;

using CommunityToolkit.Mvvm.Messaging;
using HostBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Impl;
using System;
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
            .ConfigureServices(
                (_, services) =>
                {
                    services.AddSingleton<IThemeService, ThemeServiceImpl>();
                    services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);
                    services.AddSingleton<IGuessValidationService, GuessValidationServiceImpl>();
                    services.AddSingleton<IWordProvider, WordProviderImpl>();
                })
            .AddModels()
            .AddViewModels()
            .AddViews()
            .Build();
    }

    #endregion

    #region Methods

    private static Uri GetThemeSource(IThemeService.Theme theme)
    {
        return theme switch
        {
            IThemeService.Theme.Dark => new Uri("/Themes/dark.xaml", UriKind.Relative),
            IThemeService.Theme.DarkHighContrast => new Uri("/Themes/darkHighContrast.xaml", UriKind.Relative),
            IThemeService.Theme.Light => new Uri("/Themes/light.xaml", UriKind.Relative),
            IThemeService.Theme.LightHighContrast => new Uri("/Themes/lightHighContrast.xaml", UriKind.Relative),
            _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null)
        };
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Clear the service provider from the viewmodel locator
        ViewModelLocator.ClearServiceProvider();

        // Stop and dispose of the host
        this._host.Dispose();

        base.OnExit(e);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        // Start the host
        await this._host.StartAsync();

        // Set the service provider on the viewmodel locator
        ViewModelLocator.SetServiceProvider(this._host.Services);

        // Load all possible words
        IWordProvider wordProvider = this._host.Services.GetRequiredService<IWordProvider>();
        await wordProvider.LoadAsync();

        // Load the theme service and set the current theme
        // TODO: Load the last saved value from a settings provider
        IThemeService themeService = this._host.Services.GetRequiredService<IThemeService>();
        themeService.ThemeChanged += (_, args) => this.ChangeTheme(args.OldTheme, args.NewTheme);
        this.ChangeTheme(IThemeService.Theme.Dark, themeService.GetCurrentTheme());

        // Create and show our main window.
        this.MainWindow = this._host.Services.GetRequiredService<MainWindow>();
        this.MainWindow.Show();

        base.OnStartup(e);
    }

    private void ChangeTheme(IThemeService.Theme? oldTheme, IThemeService.Theme newTheme)
    {
        if (oldTheme != null)
        {
            // TODO Replace with fixed below
            this.Resources.MergedDictionaries.Clear();

            //Uri source = this.GetThemeSource(oldTheme.Value);
            //ResourceDictionary matchingDictionary = this.Resources.MergedDictionaries.Single(x => x.Source.OriginalString == source.OriginalString);
            //this.Resources.MergedDictionaries.Remove(matchingDictionary);
        }

        Uri newThemeSource = GetThemeSource(newTheme);
        this.Resources.MergedDictionaries.Add(
            new ResourceDictionary
            {
                Source = newThemeSource
            });
    }

    #endregion
}