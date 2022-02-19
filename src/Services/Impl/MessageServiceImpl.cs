namespace WPFordle.Services.Impl;

using System;
using System.Windows.Threading;

public class MessageServiceImpl : IMessageService
{
    #region Events

    public event EventHandler<IMessageService.Message>? MessageExpired;

    public event EventHandler<IMessageService.Message>? MessageFired;

    #endregion

    #region Methods

    public void FireMessage(string message)
    {
        IMessageService.Message messageItem = new(message);
        this.MessageFired?.Invoke(this, messageItem);

        DispatcherTimer dispatcherTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(1000),
            Tag = messageItem
        };
        dispatcherTimer.Tick += this.OnMessageExpired;
        dispatcherTimer.Start();
    }

    private void OnMessageExpired(object? sender, EventArgs e)
    {
        DispatcherTimer dispatcherTimer = (DispatcherTimer)sender;
        dispatcherTimer.Stop();
        dispatcherTimer.Tick -= this.OnMessageExpired;

        IMessageService.Message messageItem = (IMessageService.Message)dispatcherTimer.Tag;
        this.MessageExpired?.Invoke(this, messageItem);
    }

    #endregion
}