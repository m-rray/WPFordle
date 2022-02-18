namespace WPFordle.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GameModel : ObservableModel
{
    #region Fields

    private int _currentGuessIndex;

    private int _currentLetterIndex;

    private bool _gameOver;

    private bool _isCommittingGuess;

    #endregion

    #region Constructors

    public GameModel(WordModel targetWord)
    {
        this.TargetWord = targetWord;
        this.Guesses = Enumerable.Range(0, Constants.MaximumGuesses).Select(x => new WordModel()).ToList();
    }

    #endregion

    #region Properties

    public WordModel TargetWord { get; }

    public IReadOnlyCollection<WordModel> Guesses { get; }

    #endregion

    #region Methods

    public bool GuessLetter(char letter)
    {
        if (this._isCommittingGuess)
        {
            return false;
        }

        if (this._gameOver)
        {
            return false;
        }

        if (this._currentLetterIndex == Constants.WordLength)
        {
            return false;
        }

        WordModel currentGuess = this.Guesses.ElementAt(this._currentGuessIndex);
        LetterModel currentLetterGuess = currentGuess.Letters.ElementAt(this._currentLetterIndex);
        currentLetterGuess.Character = letter;
        this._currentLetterIndex++;

        return true;
    }

    public bool DeleteLastCharacter()
    {
        if (this._isCommittingGuess)
        {
            return false;
        }

        if (this._gameOver)
        {
            return false;
        }

        if (this._currentLetterIndex == 0)
        {
            return false;
        }

        this._currentLetterIndex--;
        WordModel currentGuess = this.Guesses.ElementAt(this._currentGuessIndex);
        LetterModel currentLetterGuess = currentGuess.Letters.ElementAt(this._currentLetterIndex);
        currentLetterGuess.Character = null;

        return true;
    }

    public async Task<(bool, GuessResult)> TryCommitGuessAsync()
    {
        if (this._isCommittingGuess)
        {
            return (false, default);
        }

        this._isCommittingGuess = true;

        try
        {
            if (this._gameOver)
            {
                return (false, default);
            }

            if (this._currentLetterIndex != Constants.WordLength)
            {
                return (false, default);
            }

            WordModel currentGuess = this.Guesses.ElementAt(this._currentGuessIndex);
            await currentGuess.ValidateAsync(this.TargetWord);
            this._currentGuessIndex++;
            this._currentLetterIndex = 0;

            GuessResult guessResult = currentGuess.Letters.All(x => x.Result == LetterResult.RightLetterRightPlace)
                ? GuessResult.Correct
                : GuessResult.Incorrect;

            if (guessResult == GuessResult.Correct || this._currentGuessIndex == Constants.MaximumGuesses)
            {
                this._gameOver = true;
            }

            return (true, guessResult);
        }
        finally
        {
            this._isCommittingGuess = false;
        }
    }

    #endregion
}