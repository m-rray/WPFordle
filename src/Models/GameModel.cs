namespace WPFordle.Models;

using Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GameModel : ObservableModel
{
    #region Fields

    private readonly IGuessValidationService _guessValidationService;
    private readonly IWordService _wordService;
    private int _currentGuessIndex;
    private int _currentLetterIndex;
    private bool _gameOver;
    private bool _isCommittingGuess;

    #endregion

    #region Constructors

    public GameModel(WordModel targetWord, IGuessValidationService guessValidationService, IWordService wordService)
    {
        ArgumentNullException.ThrowIfNull(guessValidationService);
        ArgumentNullException.ThrowIfNull(wordService);

        this._guessValidationService = guessValidationService;
        this._wordService = wordService;
        this.TargetWord = targetWord;
        this.Guesses = Enumerable.Range(0, Constants.MaximumGuesses)
            .Select(x => new WordModel(targetWord.Letters.Count))
            .ToList();
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

        if (this._currentLetterIndex == this.TargetWord.Letters.Count)
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

    public async Task<GuessResult?> CommitGuess()
    {
        if (this._gameOver)
        {
            return null;
        }

        if (this._isCommittingGuess)
        {
            return null;
        }

        this._isCommittingGuess = true;

        try
        {
            if (this._currentLetterIndex != this.TargetWord.Letters.Count)
            {
                return new GuessResult
                {
                    Success = false,
                    Validated = false,
                    Message = "Not enough letters"
                };
            }

            WordModel currentGuess = this.Guesses.ElementAt(this._currentGuessIndex);
            string currentWord = string.Join(null, currentGuess.Letters.Select(x => x.Character));
            bool isRecognizedWord = await this._wordService.IsRecognizedWordAsync(currentWord);
            if (!isRecognizedWord)
            {
                return new GuessResult
                {
                    Success = false,
                    Validated = false,
                    Message = "Not in word list",
                    GuessedWord = currentGuess
                };
            }

            await this._guessValidationService.ValidateGuessAsync(currentGuess, this.TargetWord);
            this._currentGuessIndex++;
            this._currentLetterIndex = 0;

            bool success = currentGuess.Letters.All(x => x.State == LetterState.RightLetterRightPlace);
            GuessResult guessResult = new()
            {
                Success = success,
                Validated = true,
                Message = success ? "Genius" : null,
                GuessedWord = currentGuess
            };

            if (guessResult.Success || this._currentGuessIndex == Constants.MaximumGuesses)
            {
                this._gameOver = true;
            }

            return guessResult;
        }
        finally
        {
            this._isCommittingGuess = false;
        }
    }

    #endregion
}