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

    Theme GetCurrentTheme();

    void SetThemeSettings(bool dark, bool highContrast);

    #endregion
}