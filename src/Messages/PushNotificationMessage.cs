namespace WPFordle.Messages;

using CommunityToolkit.Mvvm.Messaging.Messages;

public class PushNotificationMessage : ValueChangedMessage<string>
{
    #region Constructors

    public PushNotificationMessage(string message)
        : base(message)
    {
    }

    #endregion
}