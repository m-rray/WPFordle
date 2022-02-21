namespace WPFordle.Views.Controls;

using Models;
using Models.Enums;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

public class Letter : Control
{
    #region Fields

    public static readonly DependencyProperty CharacterProperty = DependencyProperty.Register(
        nameof(Character),
        typeof(string),
        typeof(Letter),
        new PropertyMetadata());

    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
        nameof(State),
        typeof(LetterState),
        typeof(Letter),
        new FrameworkPropertyMetadata(LetterState.None, OnLetterStateChanged));

    private Border? _border;
    private ScaleTransform? _scale;

    #endregion

    #region Constructors

    static Letter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(Letter),
            new FrameworkPropertyMetadata(typeof(Letter)));
    }

    #endregion

    #region Properties

    public string Character
    {
        get => (string)this.GetValue(CharacterProperty);
        set => this.SetValue(CharacterProperty, value);
    }

    public LetterState State
    {
        get => (LetterState)this.GetValue(StateProperty);
        set => this.SetValue(StateProperty, value);
    }

    #endregion

    #region Methods

    public override void OnApplyTemplate()
    {
        this._border = this.GetTemplateChild("PART_Border") as Border;
        this._scale = this.GetTemplateChild("PART_Scale") as ScaleTransform;
        base.OnApplyTemplate();
    }

    private static void OnLetterStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Letter letter)
        {
            return;
        }

        letter.PlayRevealAnimation();
    }

    private void PlayRevealAnimation()
    {
        if (this.State == LetterState.None)
        {
            this.SetResourceReference(ForegroundProperty, "TextForegroundKey");
            this.Background = Brushes.Transparent;
            this._border.BorderThickness = new Thickness(2);
            return;
        }
        
        DoubleAnimation heightAnimation = new(0d, new Duration(TimeSpan.FromMilliseconds(Constants.TimeBetweenReveals/2d)));
        heightAnimation.Completed += (sender, args) =>
        {
            this.SetResourceReference(ForegroundProperty, "GuessedTextForegroundKey");
            this._border.BorderThickness = new Thickness(0);

            switch (this.State)
            {
                case LetterState.RightLetterRightPlace:
                    this.SetResourceReference(BackgroundProperty, "CorrectGuessKey");
                    break;
                case LetterState.RightLetterWrongPlace:
                    this.SetResourceReference(BackgroundProperty, "IndeterminateGuessKey");
                    break;
                case LetterState.WrongLetter:
                    this.SetResourceReference(BackgroundProperty, "IncorrectGuessKey");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DoubleAnimation newHeightAnimation = new(1d, new Duration(TimeSpan.FromMilliseconds(Constants.TimeBetweenReveals/2d)));
            _scale.BeginAnimation(ScaleTransform.ScaleYProperty, newHeightAnimation);
        };

        _scale.BeginAnimation(ScaleTransform.ScaleYProperty, heightAnimation);
    }

    #endregion
}