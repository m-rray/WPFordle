namespace WPFordle.Messages.Push;

using CommunityToolkit.Mvvm.Messaging.Messages;

public class PushAlertMessage : ValueChangedMessage<string>
{
    #region Constructors

    public PushAlertMessage(string message)
        : base(message)
    {
    }

    #endregion
}