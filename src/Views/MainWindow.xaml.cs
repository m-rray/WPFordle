namespace WPFordle.Views;

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Fields

    private bool _animatingLayer;

    #endregion

    #region Constructors

    public MainWindow()
    {
        this.InitializeComponent();
    }

    #endregion

    #region Methods

    private void OnCloseHelpAnimationCompleted(object? sender, EventArgs e)
    {
        this.Help.Visibility = Visibility.Collapsed;
        this.HowToPlayView.Reset();
        this._animatingLayer = false;
    }

    private void OnCloseHelpButtonClick(object sender, RoutedEventArgs e)
    {
        if (this._animatingLayer)
        {
            return;
        }

        this._animatingLayer = true;

        DoubleAnimation opacityAnimation = new(0d, new Duration(TimeSpan.FromMilliseconds(150)));
        opacityAnimation.Completed += this.OnCloseHelpAnimationCompleted;

        DoubleAnimation positionAnimation = new(50, new Duration(TimeSpan.FromMilliseconds(100)))
        {
            BeginTime = TimeSpan.FromMilliseconds(50)
        };

        this.Help.BeginAnimation(OpacityProperty, opacityAnimation);
        this.HelpPosition.BeginAnimation(TranslateTransform.YProperty, positionAnimation);
    }

    private void OnCloseSettingsAnimationCompleted(object? sender, EventArgs e)
    {
        this.Settings.Visibility = Visibility.Collapsed;
        this._animatingLayer = false;
    }

    private void OnCloseSettingsButtonClick(object sender, RoutedEventArgs e)
    {
        if (this._animatingLayer)
        {
            return;
        }

        this._animatingLayer = true;

        DoubleAnimation opacityAnimation = new(0d, new Duration(TimeSpan.FromMilliseconds(150)));
        opacityAnimation.Completed += this.OnCloseSettingsAnimationCompleted;

        DoubleAnimation positionAnimation = new(50, new Duration(TimeSpan.FromMilliseconds(100)))
        {
            BeginTime = TimeSpan.FromMilliseconds(50)
        };

        this.Settings.BeginAnimation(OpacityProperty, opacityAnimation);
        this.SettingsPosition.BeginAnimation(TranslateTransform.YProperty, positionAnimation);
    }

    private void OnHelpButtonClick(object sender, RoutedEventArgs e)
    {
        if (this._animatingLayer)
        {
            return;
        }

        this._animatingLayer = true;

        this.Help.Visibility = Visibility.Visible;

        DoubleAnimation opacityAnimation = new(1d, new Duration(TimeSpan.FromMilliseconds(200)));
        opacityAnimation.Completed += this.OnOpenHelpAnimationCompleted;

        DoubleAnimation positionAnimation = new(0, new Duration(TimeSpan.FromMilliseconds(150)));

        this.Help.BeginAnimation(OpacityProperty, opacityAnimation);
        this.HelpPosition.BeginAnimation(TranslateTransform.YProperty, positionAnimation);
    }

    private void OnOpenHelpAnimationCompleted(object? sender, EventArgs e)
    {
        this.HowToPlayView.Play();
        this._animatingLayer = false;
    }

    private void OnOpenSettingsAnimationCompleted(object? sender, EventArgs e)
    {
        this._animatingLayer = false;
    }

    private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
    {
        if (this._animatingLayer)
        {
            return;
        }

        this._animatingLayer = true;

        this.Settings.Visibility = Visibility.Visible;

        DoubleAnimation opacityAnimation = new(1d, new Duration(TimeSpan.FromMilliseconds(200)));
        opacityAnimation.Completed += this.OnOpenSettingsAnimationCompleted;

        DoubleAnimation positionAnimation = new(0, new Duration(TimeSpan.FromMilliseconds(150)));

        this.Settings.BeginAnimation(OpacityProperty, opacityAnimation);
        this.SettingsPosition.BeginAnimation(TranslateTransform.YProperty, positionAnimation);
    }

    #endregion
}