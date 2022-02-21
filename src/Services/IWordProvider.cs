namespace WPFordle.Services;

using System.Threading.Tasks;

public interface IWordProvider
{
    #region Methods

    public string GetDailyWord();

    public bool IsRecognizedWord(string word);

    public Task LoadAsync();

    #endregion
}