namespace WPFordle.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using Services;
using System;

public class SettingsViewModel : ObservableObject
{
    #region Fields

    private readonly IThemeService _themeService;
    private bool _darkTheme;
    private bool _highContrast;

    #endregion

    #region Constructors

    public SettingsViewModel(IThemeService themeService)
    {
        ArgumentNullException.ThrowIfNull(themeService);

        this._themeService = themeService;
        IThemeService.Theme currentTheme = themeService.GetCurrentTheme();

        switch (currentTheme)
        {
            case IThemeService.Theme.Dark:
                this._darkTheme = true;
                this._highContrast = false;
                break;
            case IThemeService.Theme.DarkHighContrast:
                this._darkTheme = true;
                this._highContrast = true;
                break;
            case IThemeService.Theme.Light:
                this._darkTheme = false;
                this._highContrast = false;
                break;
            case IThemeService.Theme.LightHighContrast:
                this._darkTheme = false;
                this._highContrast = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region Properties

    public bool DarkTheme
    {
        get => this._darkTheme;
        set
        {
            this._darkTheme = value;
            this.OnPropertyChanged();
            this._themeService.SetThemeSettings(value, this.HighContrast);
        }
    }

    public bool HighContrast
    {
        get => this._highContrast;
        set
        {
            this._highContrast = value;
            this.OnPropertyChanged();
            this._themeService.SetThemeSettings(this.DarkTheme, value);
        }
    }

    #endregion
}