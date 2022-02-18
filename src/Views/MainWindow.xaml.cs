namespace WPFordle.Views;

using System;
using System.Windows;
using System.Windows.Input;
using ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Constructors

    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        this.DataContext = mainWindowViewModel;
        this.InitializeComponent();
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        ((MainWindowViewModel)DataContext).InputCharacterCommand.NotifyCanExecuteChanged();
    }

    #endregion
}