namespace WPFordle.Models;

public class GuessResult
{
    #region Properties

    public WordModel? GuessedWord { get; set; }

    public string? Message { get; init; }

    public bool Success { get; init; }

    public bool Validated { get; init; }

    #endregion
}