namespace WPFordle.Views.Controls;

using System.Windows;

public class Layer : Overlay
{
    #region Fields

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

    #endregion

    #region Constructors

    static Layer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(Layer),
            new FrameworkPropertyMetadata(typeof(Layer)));
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

    protected override FrameworkElement AnimatedElement => this;

    #endregion
}