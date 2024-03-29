﻿namespace WPFordle.ViewModels;

using Models;
using Models.Enums;

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

    public LetterState State => this.Model.State;

    #endregion
}