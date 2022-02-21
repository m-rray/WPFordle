namespace WPFordle.Messages;

using CommunityToolkit.Mvvm.Messaging.Messages;
using Models.Enums;

public class LetterGuessedMessage : ValueChangedMessage<char>
{
    #region Constructors

    public LetterGuessedMessage(char value, LetterState letterState)
        : base(value)
    {
        this.LetterState = letterState;
    }

    #endregion

    #region Properties

    public LetterState LetterState { get; }

    #endregion
}