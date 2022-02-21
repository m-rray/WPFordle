namespace WPFordle.Services.Impl;

using System;

public class ThemeServiceImpl : IThemeService
{
    #region Fields

    private IThemeService.Theme _currentTheme = IThemeService.Theme.Dark;

    #endregion

    #region Events

    public event EventHandler<(IThemeService.Theme? OldTheme, IThemeService.Theme NewTheme)>? ThemeChanged;

    #endregion

    #region Methods

    public IThemeService.Theme GetCurrentTheme()
    {
        return this._currentTheme;
    }

    public void SetThemeSettings(bool dark, bool highContrast)
    {
        IThemeService.Theme oldTheme = this._currentTheme;
        IThemeService.Theme newTheme;
        if (dark)
        {
            newTheme = highContrast
                ? IThemeService.Theme.DarkHighContrast
                : IThemeService.Theme.Dark;
        }
        else
        {
            newTheme = highContrast
                ? IThemeService.Theme.LightHighContrast
                : IThemeService.Theme.Light;
        }

        this._currentTheme = newTheme;
        this.ThemeChanged?.Invoke(this, (oldTheme, newTheme));
    }

    #endregion
}