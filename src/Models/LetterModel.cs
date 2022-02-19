namespace WPFordle.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using Enums;

public partial class LetterModel : ObservableModel
{
    #region Fields

    [ObservableProperty]
    private char? _character;

    [ObservableProperty]
    private LetterResult _result;

    #endregion

    #region Constructors

    public LetterModel()
    {
    }

    public LetterModel(char character)
    {
        this._character = character;
    }

    #endregion
}