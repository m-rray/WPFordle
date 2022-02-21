namespace WPFordle.Models;

using System.Collections.Generic;
using System.Linq;

public class WordModel : ObservableModel
{
    #region Constructors

    public WordModel(int wordLength)
    {
        this.Letters = Enumerable.Range(0, wordLength).Select(x => new LetterModel()).ToList();
    }

    public WordModel(string word)
    {
        this.Letters = word.ToCharArray().Select(x => new LetterModel(x)).ToList();
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<LetterModel> Letters { get; }

    #endregion
}