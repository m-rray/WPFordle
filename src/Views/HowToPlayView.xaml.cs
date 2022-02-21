namespace WPFordle.Views;

using Models.Enums;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for HowToPlayView.xaml
/// </summary>
public partial class HowToPlayView : UserControl
{
    #region Constructors

    public HowToPlayView()
    {
        this.InitializeComponent();
    }

    #endregion

    #region Methods

    public void Play()
    {
        this.CorrectLetter.State = LetterState.RightLetterRightPlace;
        this.IndeterminateLetter.State = LetterState.RightLetterWrongPlace;
        this.WrongLetter.State = LetterState.WrongLetter;
    }

    public void Reset()
    {
        this.CorrectLetter.State = LetterState.None;
        this.IndeterminateLetter.State = LetterState.None;
        this.WrongLetter.State = LetterState.None;
    }

    #endregion
}