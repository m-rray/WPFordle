namespace WPFordle.Services;

using System;

public interface IMessageService
{
    #region Events

    event EventHandler<Message>? MessageFired;

    event EventHandler<Message>? MessageExpired;

    #endregion

    #region Methods

    void FireMessage(string message);

    #endregion

    public class Message
    {
        #region Constructors

        public Message(string text)
        {
            this.Text = text;
        }

        #endregion

        #region Properties

        public string Text { get; }

        #endregion
    }
}