namespace WPFordle.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using System;
using System.ComponentModel;

public abstract class ObservableViewModel<T> : ObservableObject, IDisposable
    where T : ObservableModel
{
    #region Fields

    private readonly bool _forwardModelPropertyChanges;
    private bool _disposed;

    #endregion

    #region Constructors

    protected ObservableViewModel(T model, bool forwardPropertyChanged = true)
    {
        this.Model = model ?? throw new ArgumentNullException(nameof(model));
        this._forwardModelPropertyChanges = forwardPropertyChanged;

        model.PropertyChanged += this.OnModelPropertyChanged;
    }

    #endregion

    #region Properties

    public T Model { get; }

    #endregion

    #region Methods

    protected virtual void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (this._forwardModelPropertyChanges)
        {
            this.OnPropertyChanged(e);
        }
    }

    public void Dispose()
    {
        if (this._disposed)
        {
            throw new ObjectDisposedException("Object already disposed.");
        }

        this._disposed = true;
        this.Model.PropertyChanged -= this.OnModelPropertyChanged;
    }

    #endregion
}