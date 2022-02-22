namespace WPFordle.Views.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

public abstract class Overlay : ContentControl
{
    #region Fields

    public static readonly RoutedEvent CloseRequestedEvent =
        EventManager.RegisterRoutedEvent(
                                         nameof(CloseRequested),
                                         RoutingStrategy.Bubble,
                                         typeof(EventHandler<RoutedEventArgs>),
                                         typeof(Overlay));

    private Button? _closeButton;

    #endregion

    #region Events

    public event EventHandler<RoutedEventArgs> CloseRequested
    {
        add => this.AddHandler(CloseRequestedEvent, value);
        remove => this.RemoveHandler(CloseRequestedEvent, value);
    }

    #endregion

    #region Properties

    protected abstract FrameworkElement AnimatedElement { get; }

    #endregion

    #region Methods

    public void Hide(Action? onHideAnimationCompleted = null)
    {
        DoubleAnimation opacityAnimation = new(0d, new Duration(TimeSpan.FromMilliseconds(150)));
        opacityAnimation.Completed += (_, _) =>
                                      {
                                          this.Visibility = Visibility.Collapsed;
                                          onHideAnimationCompleted?.Invoke();
                                      };

        DoubleAnimation positionAnimation = new(50, new Duration(TimeSpan.FromMilliseconds(100)))
                                            {
                                                BeginTime = TimeSpan.FromMilliseconds(50)
                                            };

        this.AnimatedElement.RenderTransformOrigin = new Point(0.5, 0.5);
        this.AnimatedElement.RenderTransform = new TranslateTransform();

        this.AnimatedElement.BeginAnimation(OpacityProperty, opacityAnimation);
        this.AnimatedElement.RenderTransform.BeginAnimation(TranslateTransform.YProperty, positionAnimation);
    }

    public override void OnApplyTemplate()
    {
        if (this._closeButton != null)
        {
            this._closeButton.Click -= this.OnCloseButtonClick;
        }

        this._closeButton = this.GetTemplateChild("PART_CloseButton") as Button;
        this._closeButton.Click += this.OnCloseButtonClick;

        base.OnApplyTemplate();
    }

    public void Show(Action? onShowAnimationCompleted = null)
    {
        this.Visibility = Visibility.Visible;

        DoubleAnimation opacityAnimation = new(1d, new Duration(TimeSpan.FromMilliseconds(200)));
        opacityAnimation.Completed += (_, _) => onShowAnimationCompleted?.Invoke();

        DoubleAnimation positionAnimation = new(0, new Duration(TimeSpan.FromMilliseconds(150)));

        this.AnimatedElement.RenderTransformOrigin = new Point(0.5, 0.5);
        this.AnimatedElement.RenderTransform = new TranslateTransform
                                               {
                                                   Y = 50d
                                               };

        this.AnimatedElement.BeginAnimation(OpacityProperty, opacityAnimation);
        this.AnimatedElement.RenderTransform.BeginAnimation(TranslateTransform.YProperty, positionAnimation);
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        this.RaiseEvent(new RoutedEventArgs(CloseRequestedEvent, this));
    }

    #endregion
}