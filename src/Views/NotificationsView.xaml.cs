namespace WPFordle.Views;

using Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using ViewModels;

/// <summary>
/// Interaction logic for NotificationsView.xaml
/// </summary>
public partial class NotificationsView : UserControl
{
    #region Constructors

    public NotificationsView()
    {
        this.InitializeComponent();

        if (this.DataContext is not NotificationsViewModel notificationsViewModel)
        {
            throw new InvalidOperationException(
                "Notifications view must have a datacontext of type NotificationsViewModel");
        }

        notificationsViewModel.Notifications.CollectionChanged += this.OnNotificationsCollectionChanged;
    }

    #endregion

    #region Methods

    private void OnCardOpacityAnimationCompleted(object? sender, EventArgs e)
    {
        this.ItemsControl.Items.RemoveAt(this.ItemsControl.Items.Count - 1);
    }

    private void OnNotificationsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            for (int i = 0; i < e.OldItems.Count; i++)
            {
                Card card = (Card)this.ItemsControl.Items[^1];
                DoubleAnimation opacityAnimation = new(0d, new Duration(TimeSpan.FromMilliseconds(250)));
                opacityAnimation.Completed += this.OnCardOpacityAnimationCompleted;

                card.BeginAnimation(OpacityProperty, opacityAnimation);
            }
        }

        if (e.NewItems != null)
        {
            List<string> newItems = e.NewItems.OfType<string>().ToList();
            foreach (string newItem in newItems)
            {
                this.ItemsControl.Items.Insert(
                    0,
                    new Card
                    {
                        Margin = new Thickness(5),
                        Text = newItem,
                        Opacity = 1d
                    });
            }
        }
    }

    #endregion
}