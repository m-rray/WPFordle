namespace WPFordle.ViewModels;

using Models;
using System.Collections.Generic;
using System.Linq;

public class WordViewModel : ObservableViewModel<WordModel>
{
    #region Constructors

    public WordViewModel(WordModel model)
        : base(model)
    {
        this.Letters = model.Letters.Select(x => new LetterViewModel(x)).ToList();
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<LetterViewModel> Letters { get; }

    #endregion
}