namespace WPFordle.ViewModels;

using Models;

public class LetterViewModel : ObservableViewModel<LetterModel>
{
    #region Constructors

    public LetterViewModel(LetterModel model)
        : base(model)
    {
    }

    #endregion

    #region Properties

    public char? Character => this.Model.Character;

    public LetterResult Result => this.Model.Result;

    #endregion
}