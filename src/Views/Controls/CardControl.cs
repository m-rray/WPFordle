namespace WPFordle.Views.Controls;

using System.Windows;
using System.Windows.Controls;

public class CardControl : Control
{
    #region Fields

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(CardControl),
        new PropertyMetadata());

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(CardControl),
        new PropertyMetadata());

    #endregion

    #region Constructors

    static CardControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(CardControl),
            new FrameworkPropertyMetadata(typeof(CardControl)));
    }

    #endregion

    #region Properties

    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }

    #endregion
}