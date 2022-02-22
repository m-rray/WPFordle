namespace WPFordle.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class StatisticsViewModel : ObservableObject
{
    #region Fields

    [ObservableProperty]
    private int _currentStreak;

    [ObservableProperty]
    private int _maxStreak;

    [ObservableProperty]
    private int _played;

    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(WinPercentage))]
    private int _won;

    #endregion

    #region Properties

    public int WinPercentage => this.Won / this.Played * 100;

    #endregion
}