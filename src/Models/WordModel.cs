namespace WPFordle.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class WordModel : ObservableModel
{
    #region Constructors

    public WordModel()
    {
        this.Letters = Enumerable.Range(0, Constants.WordLength).Select(x => new LetterModel(x)).ToList();
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<LetterModel> Letters { get; }

    #endregion

    #region Methods

    public async Task ValidateAsync(WordModel targetWord)
    {
        foreach (LetterModel letterModel in this.Letters)
        {
            letterModel.Validate(targetWord);
            await Task.Delay(Constants.TimeBetweenReveals);
        }
    }

    #endregion
}