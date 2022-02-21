namespace WPFordle.Messages;

using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Windows.Input;

public class KeyPressedMessage : ValueChangedMessage<Key>
{
    #region Constructors

    public KeyPressedMessage(Key value)
        : base(value)
    {
    }

    #endregion
}