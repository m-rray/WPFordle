namespace WPFordle.Views;

using CommunityToolkit.Mvvm.Messaging;
using Controls;
using Messages.Push;
using Models.Enums;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Fields

    private bool _animatingLayer;

    #endregion

    #region Constructors

    public MainWindow(
        MainWindowViewModel mainWindowViewModel,
        IMessenger messenger)
    {
        ArgumentNullException.ThrowIfNull(messenger);

        this.DataContext = mainWindowViewModel;
        this.InitializeComponent();

        messenger.Register<PushAlertMessage>(
            this,
            (_, message) => this.DisplayMessageCard(message.Value));
    }

    #endregion

    #region Methods

    private void DisplayMessageCard(string message)
    {
        Card card = new()
        {
            Margin = new Thickness(5),
            Text = message,
            Opacity = 1d
        };

        this.CardView.Items.Insert(0, card);

        DispatcherTimer dispatcherTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(1000)
        };
        dispatcherTimer.Tick += this.OnCardExpired;
        dispatcherTimer.Start();
    }

    private void OnCardExpired(object? sender, EventArgs e)
    {
        if (sender is DispatcherTimer timer)
        {
            timer.Stop();
            timer.Tick -= this.OnCardExpired;
        }

        // We know that this is the last card, without needing to check.
        Card card = (Card)this.CardView.Items[^1];
        DoubleAnimation opacityAnimation = new(0d, new Duration(TimeSpan.FromMilliseconds(250)));
        opacityAnimation.Completed += this.OnCardOpacityAnimationCompleted;

        card.BeginAnimation(OpacityProperty, opacityAnimation);
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        ((MainWindowViewModel)this.DataContext).InputCharacterCommand.NotifyCanExecuteChanged();
    }

    private void OnCardOpacityAnimationCompleted(object? sender, EventArgs e)
    {
        this.CardView.Items.RemoveAt(this.CardView.Items.Count - 1);
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

    private void OnOpenSettingsAnimationCompleted(object? sender, EventArgs e)
    {
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

    private void OnCloseSettingsAnimationCompleted(object? sender, EventArgs e)
    {
        this.Settings.Visibility = Visibility.Collapsed;
        this._animatingLayer = false;
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

    private void OnCloseHelpAnimationCompleted(object? sender, EventArgs e)
    {
        this.Help.Visibility = Visibility.Collapsed;
        this.HowToPlayView.Reset();
        this._animatingLayer = false;
    }

    #endregion
}