namespace WPFordle.Views.Controls;

using Models.Enums;
using System.Windows;
using System.Windows.Controls;

internal class KeyboardLetterButton : Button
{
    #region Fields

    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
        nameof(State),
        typeof(LetterState),
        typeof(KeyboardLetterButton),
        new FrameworkPropertyMetadata(LetterState.None));

    #endregion

    #region Constructors

    static KeyboardLetterButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(KeyboardLetterButton),
            new FrameworkPropertyMetadata(typeof(KeyboardLetterButton)));
    }

    #endregion

    #region Properties

    public LetterState State
    {
        get => (LetterState)this.GetValue(StateProperty);
        set => this.SetValue(StateProperty, value);
    }

    #endregion
}