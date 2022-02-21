namespace WPFordle.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Messages;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

public class NotificationsViewModel : ObservableObject
{
    #region Fields

    private static readonly TimeSpan NotificationLifespan = TimeSpan.FromSeconds(1);

    #endregion

    #region Constructors

    public NotificationsViewModel(IMessenger messenger)
    {
        ArgumentNullException.ThrowIfNull(messenger);

        this.Notifications = new ObservableCollection<string>();

        messenger.Register<PushNotificationMessage>(this, (_, message) => this.PushNotification(message.Value));
    }

    #endregion

    #region Properties

    public ObservableCollection<string> Notifications { get; }

    #endregion

    #region Methods

    private void OnCardExpired(object? sender, EventArgs e)
    {
        if (sender is DispatcherTimer timer)
        {
            timer.Stop();
            timer.Tick -= this.OnCardExpired;
        }

        this.Notifications.RemoveAt(this.Notifications.Count - 1);
    }

    private void PushNotification(string message)
    {
        this.Notifications.Insert(0, message);
        DispatcherTimer dispatcherTimer = new()
        {
            Interval = NotificationLifespan
        };
        dispatcherTimer.Tick += this.OnCardExpired;
        dispatcherTimer.Start();
    }

    #endregion
}