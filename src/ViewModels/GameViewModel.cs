namespace WPFordle.ViewModels;

using CommunityToolkit.Mvvm.Messaging;
using Messages;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

public class GameViewModel : ObservableViewModel<GameModel>
{
    #region Fields

    private readonly IMessenger _messenger;

    #endregion

    #region Constructors

    public GameViewModel(GameModel model, IMessenger messenger)
        : base(model)
    {
        ArgumentNullException.ThrowIfNull(messenger);

        this._messenger = messenger;
        this.Guesses = model.Guesses.Select(x => new WordViewModel(x)).ToList();

        this._messenger.Register<GameViewModel, KeyPressedMessage>(this, (_, m) => this.OnKeyPressed(m.Value));
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<WordViewModel> Guesses { get; }

    #endregion

    #region Methods

    private async void CommitGuess()
    {
        GuessResult? guess = await this.Model.CommitGuess();
        if (guess == null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(guess.Message))
        {
            this._messenger.Send(new PushNotificationMessage(guess.Message));
        }

        if (guess.Validated && guess.GuessedWord != null)
        {
            foreach (LetterModel guessedWordLetter in guess.GuessedWord.Letters)
            {
                this._messenger.Send(
                    new LetterGuessedMessage(
                        guessedWordLetter.Character!.Value,
                        guessedWordLetter.State));
            }
        }
    }

    private void DeleteLastLetter()
    {
        this.Model.DeleteLastCharacter();
    }

    private void GuessLetter(char character)
    {
        this.Model.GuessLetter(character);
    }

    private void OnKeyPressed(Key key)
    {
        switch (key)
        {
            case Key.Enter:
                this.CommitGuess();
                break;
            case Key.Back:
                this.DeleteLastLetter();
                break;
            case Key.A:
            case Key.B:
            case Key.C:
            case Key.D:
            case Key.E:
            case Key.F:
            case Key.G:
            case Key.H:
            case Key.I:
            case Key.J:
            case Key.K:
            case Key.L:
            case Key.M:
            case Key.N:
            case Key.O:
            case Key.P:
            case Key.Q:
            case Key.R:
            case Key.S:
            case Key.T:
            case Key.U:
            case Key.V:
            case Key.W:
            case Key.X:
            case Key.Y:
            case Key.Z:
                this.GuessLetter(key.ToString().ToUpper().ToCharArray()[0]);
                break;
        }
    }

    #endregion
}