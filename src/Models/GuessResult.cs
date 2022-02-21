namespace WPFordle.Models;

public class GuessResult
{
    #region Properties

    public bool Success { get; init; }

    public bool Validated { get; init; }

    public string? Message { get; init; }

    public WordModel? GuessedWord { get; set; }

    #endregion
}