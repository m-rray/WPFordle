namespace WPFordle.Views.Controls;

using System;
using System.Windows;
using System.Windows.Controls;

public class Layer : ContentControl
{
    #region Fields

    public static readonly RoutedEvent CloseRequestedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(CloseRequested),
            RoutingStrategy.Bubble,
            typeof(EventHandler<RoutedEventArgs>),
            typeof(Layer));

    public static readonly DependencyProperty MaxInnerWidthProperty = DependencyProperty.Register(
        nameof(MaxInnerWidth),
        typeof(double),
        typeof(Layer),
        new PropertyMetadata());

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(Layer),
        new PropertyMetadata());

    private Button? _closeButton;

    #endregion

    #region Constructors

    static Layer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(Layer),
            new FrameworkPropertyMetadata(typeof(Layer)));
    }

    #endregion

    #region Events

    public event EventHandler<RoutedEventArgs> CloseRequested
    {
        add => this.AddHandler(CloseRequestedEvent, value);
        remove => this.RemoveHandler(CloseRequestedEvent, value);
    }

    #endregion

    #region Properties

    public double MaxInnerWidth
    {
        get => (double)this.GetValue(MaxInnerWidthProperty);
        set => this.SetValue(MaxInnerWidthProperty, value);
    }

    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    #endregion

    #region Methods

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

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        this.RaiseEvent(new RoutedEventArgs(CloseRequestedEvent, this));
    }

    #endregion
}