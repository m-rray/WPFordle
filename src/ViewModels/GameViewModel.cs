namespace WPFordle.ViewModels;

using Models;
using System.Collections.Generic;
using System.Linq;

public class GameViewModel : ObservableViewModel<GameModel>
{
    #region Constructors

    public GameViewModel(GameModel model)
        : base(model)
    {
        this.Guesses = model.Guesses.Select(x => new WordViewModel(x)).ToList();
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<WordViewModel> Guesses { get; }

    #endregion
}