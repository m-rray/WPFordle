namespace WPFordle.Views.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public class Dialog : Overlay
{
    #region Fields

    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
     nameof(CornerRadius),
     typeof(CornerRadius),
     typeof(Dialog),
     new PropertyMetadata());

    public static readonly DependencyProperty MaxDialogWidthProperty = DependencyProperty.Register(
     nameof(MaxDialogWidth),
     typeof(double),
     typeof(Dialog),
     new PropertyMetadata());

    public static readonly DependencyProperty DialogBackgroundProperty = DependencyProperty.Register(
     nameof(DialogBackground),
     typeof(Brush),
     typeof(Dialog),
     new PropertyMetadata());

    private Grid? _dialog;

    #endregion

    #region Constructors

    static Dialog()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
                                                 typeof(Dialog),
                                                 new FrameworkPropertyMetadata(typeof(Dialog)));
    }

    #endregion

    #region Properties

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)this.GetValue(CornerRadiusProperty);
        set => this.SetValue(CornerRadiusProperty, value);
    }


    public double MaxDialogWidth
    {
        get => (double)this.GetValue(MaxDialogWidthProperty);
        set => this.SetValue(MaxDialogWidthProperty, value);
    }

    public Brush DialogBackground
    {
        get => (Brush)this.GetValue(DialogBackgroundProperty);
        set => this.SetValue(DialogBackgroundProperty, value);
    }

    protected override FrameworkElement AnimatedElement => this._dialog ?? throw new InvalidOperationException();

    #endregion

    #region Methods

    public override void OnApplyTemplate()
    {
        this._dialog = this.GetTemplateChild("PART_Dialog") as Grid;

        base.OnApplyTemplate();
    }

    #endregion
}