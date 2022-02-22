namespace WPFordle.Views;

using System;
using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Constructors

    public MainWindow()
    {
        this.InitializeComponent();
    }

    #endregion

    #region Methods

    private void CloseHelpLayer(object sender, RoutedEventArgs e)
    {
        this.HelpLayer.Hide(() => this.HowToPlayLayerView.Reset());
    }

    private void CloseSettingsLayer(object sender, RoutedEventArgs e)
    {
        this.SettingsLayer.Hide();
    }

    private void ShowHelpLayer(object sender, RoutedEventArgs e)
    {
        this.HelpLayer.Show(() => this.HowToPlayLayerView.Play());
    }

    private void ShowSettingsLayer(object sender, RoutedEventArgs e)
    {
        this.SettingsLayer.Show();
    }

    private void CloseHelpDialog(object sender, RoutedEventArgs e)
    {
        this.HelpDialog.Hide(() => this.HowToPlayDialogView.Reset());
    }

    private void ShowHelpDialog()
    {
        this.HelpDialog.Show(() => this.HowToPlayDialogView.Play());
    }

    private void CloseStatisticsDialog(object sender, RoutedEventArgs e)
    {
        this.StatisticsDialog.Hide();
    }

    private void ShowStatisticsDialog(object sender, RoutedEventArgs e)
    {
        this.StatisticsDialog.Show();
    }

    protected override void OnContentRendered(EventArgs e)
    {
        this.ShowHelpDialog();
        base.OnContentRendered(e);
    }

    #endregion
}