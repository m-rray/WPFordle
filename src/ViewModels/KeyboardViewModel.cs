namespace WPFordle.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Messages;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

public class KeyboardViewModel : ObservableObject
{
    #region Fields

    private static readonly Key[] KeyboardRow1 =
    {
        Key.Q,
        Key.W,
        Key.E,
        Key.R,
        Key.T,
        Key.Y,
        Key.U,
        Key.I,
        Key.O,
        Key.P
    };

    private static readonly Key[] KeyboardRow2 =
    {
        Key.A,
        Key.S,
        Key.D,
        Key.F,
        Key.G,
        Key.H,
        Key.J,
        Key.K,
        Key.L
    };

    private static readonly Key[] KeyboardRow3 =
    {
        Key.Z,
        Key.X,
        Key.C,
        Key.V,
        Key.B,
        Key.N,
        Key.M
    };

    #endregion

    #region Constructors

    public KeyboardViewModel(IMessenger messenger)
    {
        IReadOnlyCollection<KeyViewModelBase> rowOneKeys =
            KeyboardRow1.Select(x => new LetterKeyViewModel(x, messenger)).ToList();

        IReadOnlyCollection<KeyViewModelBase> rowTwoKeys =
            KeyboardRow2.Select(x => new LetterKeyViewModel(x, messenger)).ToList();

        List<KeyViewModelBase> rowThreeKeys =
            KeyboardRow3.Select(x => new LetterKeyViewModel(x, messenger)).ToList<KeyViewModelBase>();
        rowThreeKeys.Insert(0, new EnterKeyViewModel(messenger));
        rowThreeKeys.Add(new DeleteKeyViewModel(messenger));

        this.Keys = new List<IReadOnlyCollection<KeyViewModelBase>>
        {
            rowOneKeys,
            rowTwoKeys,
            rowThreeKeys
        };

        messenger.Register<KeyboardViewModel, LetterGuessedMessage>(
            this,
            (_, m) => this.OnLetterGuessed(m.Value, m.LetterState));
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<IReadOnlyCollection<KeyViewModelBase>> Keys { get; }

    #endregion

    #region Methods

    private void OnLetterGuessed(char character, LetterState letterState)
    {
        LetterKeyViewModel? match = this.Keys.SelectMany(x => x)
            .OfType<LetterKeyViewModel>()
            .FirstOrDefault(x => x.Label == character);

        match?.UpdateLetterResult(letterState);
    }

    #endregion
}

public abstract partial class KeyViewModelBase : ObservableObject
{
    #region Fields

    private readonly Key _key;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private LetterState _letterState;

    #endregion

    #region Constructors

    protected KeyViewModelBase(Key key, IMessenger messenger)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(messenger);

        this._key = key;
        this._messenger = messenger;
    }

    #endregion

    #region Methods

    [ICommand]
    public void KeyPressed()
    {
        this._messenger.Send(new KeyPressedMessage(this._key));
    }

    #endregion
}

public class EnterKeyViewModel : KeyViewModelBase
{
    #region Constructors

    public EnterKeyViewModel(IMessenger messenger)
        : base(Key.Enter, messenger)
    {
    }

    #endregion
}

public class DeleteKeyViewModel : KeyViewModelBase
{
    #region Constructors

    public DeleteKeyViewModel(IMessenger messenger)
        : base(Key.Back, messenger)
    {
    }

    #endregion
}

public class LetterKeyViewModel : KeyViewModelBase
{
    #region Constructors

    public LetterKeyViewModel(Key key, IMessenger messenger)
        : base(key, messenger)
    {
        this.Label = key.ToString().ToUpper().ToCharArray()[0];
    }

    #endregion

    #region Properties

    public char Label { get; }

    #endregion

    #region Methods

    public void UpdateLetterResult(LetterState letterState)
    {
        if (letterState is LetterState.None or LetterState.WrongLetter)
        {
            this.LetterState = letterState;
        }
        else if (letterState == LetterState.RightLetterWrongPlace
                 && this.LetterState != LetterState.RightLetterRightPlace)
        {
            this.LetterState = letterState;
        }
        else
        {
            this.LetterState = letterState;
        }
    }

    #endregion
}