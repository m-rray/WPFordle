namespace WPFordle.Services;

using System;

public interface IThemeService
{
    #region Enums

    public enum Theme
    {
        Dark,
        DarkHighContrast,
        Light,
        LightHighContrast
    }

    #endregion

    #region Events

    event EventHandler<(Theme? OldTheme, Theme NewTheme)>? ThemeChanged;

    #endregion

    #region Methods

    void SetThemeSettings(bool dark, bool highContrast);

    Theme GetCurrentTheme();

    #endregion
}