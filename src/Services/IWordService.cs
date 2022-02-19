namespace WPFordle.Services;

using System.Threading.Tasks;

public interface IWordService
{
    #region Methods

    public Task<string> GetDailyWordAsync();

    public Task<bool> IsRecognizedWordAsync(string word);

    #endregion
}