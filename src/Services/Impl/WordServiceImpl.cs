namespace WPFordle.Services.Impl;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class WordServiceImpl : IWordService
{
    #region Fields

    private static readonly Uri AnswersList = new(
        "https://gist.githubusercontent.com/cfreshman/a03ef2cba789d8cf00c08f767e0fad7b/raw/5d752e5f0702da315298a6bb5a771586d6ff445c/wordle-answers-alphabetical.txt");

    private static readonly Uri AllowedGuessesList = new(
        "https://gist.githubusercontent.com/cfreshman/cdcdf777450c5b5301e439061d29694c/raw/de1df631b45492e0974f7affe266ec36fed736eb/wordle-allowed-guesses.txt");

    private readonly Lazy<Task<(IReadOnlyCollection<string> Answers, IReadOnlyCollection<string> AllowedGuesses)>>
        _words;

    #endregion

    #region Constructors

    public WordServiceImpl()
    {
        this._words =
            new Lazy<Task<(IReadOnlyCollection<string> Answers, IReadOnlyCollection<string> AllowedGuesses)>>(
                this.LoadAllWordsAsync);
    }

    #endregion

    #region Methods

    public async Task<string> GetDailyWordAsync()
    {
        (IReadOnlyCollection<string> Answers, IReadOnlyCollection<string> AllowedGuesses) words =
            await this._words.Value;

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        int hash = today.GetHashCode();
        Random random = new(hash);
        int index = random.Next(0, words.Answers.Count);

        return words.Answers.ElementAt(index);
    }

    public async Task<bool> IsRecognizedWordAsync(string word)
    {
        string key = word.ToUpper();
        (IReadOnlyCollection<string> Answers, IReadOnlyCollection<string> AllowedGuesses) words =
            await this._words.Value;
        return words.Answers.Contains(key) || words.AllowedGuesses.Contains(key);
    }

    private async Task<(IReadOnlyCollection<string> Answers, IReadOnlyCollection<string> AllowedGuesses)>
        LoadAllWordsAsync()
    {
        using HttpClient httpClient = new();

        string answersString = await httpClient.GetStringAsync(AnswersList);
        IReadOnlyCollection<string> answersList = answersString.Split('\n', '\r')
            .Select(x => x.Trim().ToUpper())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet();

        string allowedGuessesString = await httpClient.GetStringAsync(AllowedGuessesList);
        IReadOnlyCollection<string> allowedGuessesList = allowedGuessesString.Split('\n', '\r')
            .Select(x => x.Trim().ToUpper())
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet();

        return (answersList, allowedGuessesList);
    }

    #endregion
}