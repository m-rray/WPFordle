namespace WPFordle.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

public partial class LetterModel : ObservableModel
{
    #region Fields

    private readonly int _index;

    [ObservableProperty]
    private char? _character;

    [ObservableProperty]
    private LetterResult _result;

    #endregion

    #region Constructors

    public LetterModel(int index)
    {
        this._index = index;
    }

    #endregion

    #region Methods

    public void Validate(WordModel targetWord)
    {
        List<LetterModel> matchingLetters = targetWord.Letters.Where(x => x.Character == this.Character).ToList();

        if (!matchingLetters.Any())
        {
            this.Result = LetterResult.WrongLetter;
        }
        else if (matchingLetters.Any(x => x._index == this._index))
        {
            this.Result = LetterResult.RightLetterRightPlace;
        }
        else
        {
            this.Result = LetterResult.RightLetterWrongPlace;
        }
    }

    #endregion
}