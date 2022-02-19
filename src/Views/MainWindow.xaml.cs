namespace WPFordle.Views;

using Controls;
using Services;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Fields

    private bool _playingSettingsAnimation;

    #endregion

    #region Constructors

    public MainWindow(
        MainWindowViewModel mainWindowViewModel,
        IMessageService messageService)
    {
        ArgumentNullException.ThrowIfNull(messageService);
        
        this.DataContext = mainWindowViewModel;
        this.InitializeComponent();

        messageService.MessageFired += this.OnMessageFired;
        messageService.MessageExpired += this.OnMessageExpired;
    }

    #endregion

    #region Methods

    private void OnMessageFired(object? sender, IMessageService.Message e)
    {
        CardControl card = new()
        {
            Margin = new Thickness(5),
            Text = e.Text,
            Opacity = 1d
        };

        this.CardView.Items.Insert(0, card);
    }

    private void OnMessageExpired(object? sender, IMessageService.Message e)
    {
        // We know that this is the last card, without needing to check.
        CardControl card = (CardControl)this.CardView.Items[^1];
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
        if (this._playingSettingsAnimation)
        {
            return;
        }

        this.Settings.Visibility = Visibility.Visible;

        DoubleAnimation opacityAnimation = new(1d, new Duration(TimeSpan.FromMilliseconds(200)));
        opacityAnimation.Completed += this.OnOpenSettingsAnimationCompleted;

        DoubleAnimation positionAnimation = new(0, new Duration(TimeSpan.FromMilliseconds(150)));

        this.Settings.BeginAnimation(OpacityProperty, opacityAnimation);
        this.SettingsPosition.BeginAnimation(TranslateTransform.YProperty, positionAnimation);
    }

    private void OnOpenSettingsAnimationCompleted(object? sender, EventArgs e)
    {
        this._playingSettingsAnimation = false;
    }

    private void OnCloseSettingsButtonClick(object sender, RoutedEventArgs e)
    {
        if (this._playingSettingsAnimation)
        {
            return;
        }

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
        this._playingSettingsAnimation = false;
    }

    #endregion
}