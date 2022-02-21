namespace WPFordle;

using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using HostBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Impl;
using System;
using System.Linq;
using System.Windows;
using ViewModels;
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
                (host, services) =>
                {
                    services.AddSingleton<IThemeService, ThemeServiceImpl>();
                    services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);
                    services.AddSingleton<IGuessValidationService, GuessValidationServiceImpl>();
                    services.AddSingleton<IWordService, WordServiceImpl>();
                })
            .AddViewModels()
            .AddViews()
            .Build();
    }

    #endregion

    #region Methods

    protected override async void OnStartup(StartupEventArgs e)
    {
        await this._host.StartAsync();

        IThemeService themeService = this._host.Services.GetRequiredService<IThemeService>();
        themeService.ThemeChanged += this.OnThemeChanged;
        this.ChangeTheme(IThemeService.Theme.Dark, themeService.GetCurrentTheme());

        this.MainWindow = this._host.Services.GetRequiredService<MainWindow>();
        this.MainWindow.Show();

        IWordService wordService = this._host.Services.GetRequiredService<IWordService>();
        IGuessValidationService guessValidationService =
            this._host.Services.GetRequiredService<IGuessValidationService>();
        MainWindowViewModel mainWindowViewModel = this._host.Services.GetRequiredService<MainWindowViewModel>();
        await mainWindowViewModel.StartNewGameAsync(guessValidationService, wordService);

        base.OnStartup(e);
    }

    private void OnThemeChanged(object? sender, (IThemeService.Theme? OldTheme, IThemeService.Theme NewTheme) e)
    {
        this.ChangeTheme(e.OldTheme, e.NewTheme);
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

        Uri newThemeSource = this.GetThemeSource(newTheme);
        this.Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = newThemeSource
        });
    }

    private Uri GetThemeSource(IThemeService.Theme theme)
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
        this._host.Dispose();

        base.OnExit(e);
    }

    #endregion
}