namespace WPFordle.Views;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

public static class ViewModelLocator
{
    #region Fields

    public static readonly DependencyProperty AutoWireViewModelProperty = DependencyProperty.RegisterAttached(
        "AutoWireViewModel",
        typeof(bool),
        typeof(ViewModelLocator),
        new PropertyMetadata(false, AutoWireViewModelChanged));

    private static IServiceProvider? _serviceProvider;

    #endregion

    #region Methods

    public static void ClearServiceProvider()
    {
        _serviceProvider = null;
    }

    public static bool GetAutoWireViewModel(UIElement element)
    {
        return (bool)element.GetValue(AutoWireViewModelProperty);
    }

    public static void SetAutoWireViewModel(UIElement element, bool value)
    {
        element.SetValue(AutoWireViewModelProperty, value);
    }

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue)
        {
            Bind(d);
        }
    }

    private static void Bind(DependencyObject view)
    {
        if (view is not FrameworkElement frameworkElement)
        {
            return;
        }

        if (_serviceProvider == null)
        {
            throw new ServiceProviderNotSetException();
        }

        Type viewType = frameworkElement.GetType();
        Type? viewModelType = GetViewModelTypeForView(viewType);
        if (viewModelType == null)
        {
            throw new ViewModelNotFoundException(viewType);
        }

        frameworkElement.DataContext = _serviceProvider.GetRequiredService(viewModelType);
    }

    private static Type? GetViewModelTypeForView(Type viewType)
    {
        string? viewModelName = viewType.FullName?
            .Replace("View", "ViewModel")
            .Replace("Page", "ViewModel")
            .Replace("Window", "ViewModel");

        return viewModelName == null
            ? null
            : Type.GetType(viewModelName);
    }

    #endregion

    private class ServiceProviderNotSetException : Exception
    {
        #region Constructors

        public ServiceProviderNotSetException()
            : base(
                "Service provider has not been set on the ViewModelLocator. Set this before trying to auto wire view models.")
        {
        }

        #endregion
    }

    private class ViewModelNotFoundException : Exception
    {
        #region Constructors

        public ViewModelNotFoundException(Type viewType)
            : base($"Unable to determine viewmodel type for view [{viewType}]")
        {
        }

        #endregion
    }
}