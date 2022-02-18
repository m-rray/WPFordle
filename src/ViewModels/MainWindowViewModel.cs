namespace WPFordle.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    #region Fields

    [ObservableProperty]
    private GameViewModel? _game;

    #endregion

    #region Constructors

    public MainWindowViewModel()
    {
        WordModel targetWord = new();
        targetWord.Letters.ElementAt(0).Character = 'T';
        targetWord.Letters.ElementAt(1).Character = 'E';
        targetWord.Letters.ElementAt(2).Character = 'S';
        targetWord.Letters.ElementAt(3).Character = 'T';
        targetWord.Letters.ElementAt(4).Character = 'A';

        this._game = new GameViewModel(new GameModel(targetWord));
        this.Keyboard = new KeyboardViewModel(this.InputCharacterCommand, this.CommitGuessCommand, this.DeleteLastCharacterCommand);
    }

    #endregion

    #region Properties

    public KeyboardViewModel Keyboard { get; }

    #endregion

    #region Methods

    [ICommand]
    private void InputCharacter(char input)
    {
        this._game.Model.GuessLetter(input);
    }

    [ICommand]
    private async Task CommitGuess()
    {
        (bool Succeeded, GuessResult Result) guess = await this._game.Model.TryCommitGuessAsync();
        if (guess.Succeeded && guess.Result is GuessResult.Correct)
        {
            MessageBox.Show("YOU WIN!");
        }
    }

    [ICommand]
    private void DeleteLastCharacter()
    {
        this._game.Model.DeleteLastCharacter();
    }

    #endregion
}

public class KeyboardViewModel : ObservableObject
{
    #region Fields

    private static readonly char[] KeyboardRow1 =
    {
        'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P'
    };

    private static readonly char[] KeyboardRow2 =
    {
        'A','S', 'D', 'F', 'G', 'H', 'J', 'K', 'L'
    };

    private static readonly char[] KeyboardRow3 =
    {
        'Z', 'X', 'C', 'V', 'B', 'N', 'M'
    };

    #endregion

    #region Constructors

    public KeyboardViewModel(IRelayCommand<char> guessLetterCommand, IRelayCommand commitGuessCommand, IRelayCommand deleteLastGuessCommand)
    {
        IReadOnlyCollection<KeyViewModelBase> rowOneKeys =
            KeyboardRow1.Select(x => new LetterKeyViewModel(x, guessLetterCommand)).ToList();

        IReadOnlyCollection<KeyViewModelBase> rowTwoKeys =
            KeyboardRow2.Select(x => new LetterKeyViewModel(x, guessLetterCommand)).ToList();

        List<KeyViewModelBase> rowThreeKeys =
            KeyboardRow3.Select(x => new LetterKeyViewModel(x, guessLetterCommand)).ToList<KeyViewModelBase>();
        rowThreeKeys.Insert(0, new EnterKeyViewModel(commitGuessCommand));
        rowThreeKeys.Add(new DeleteKeyViewModel(deleteLastGuessCommand));

        this.Keys = new List<IReadOnlyCollection<KeyViewModelBase>> {rowOneKeys, rowTwoKeys, rowThreeKeys};
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<IReadOnlyCollection<KeyViewModelBase>> Keys { get; }

    #endregion
}

public abstract class KeyViewModelBase : ObservableObject
{
}

public class EnterKeyViewModel : KeyViewModelBase
{
    #region Constructors

    public EnterKeyViewModel(IRelayCommand commitGuessCommand)
    {
        this.CommitGuessCommand = commitGuessCommand;
    }

    #endregion

    #region Properties

    public IRelayCommand CommitGuessCommand { get; }

    #endregion
}

public class DeleteKeyViewModel : KeyViewModelBase
{
    #region Constructors

    public DeleteKeyViewModel(IRelayCommand deleteLastGuessCommand)
    {
        this.DeleteLastGuessCommand = deleteLastGuessCommand;
    }

    #endregion

    #region Properties

    public IRelayCommand DeleteLastGuessCommand { get; }

    #endregion
}

public class LetterKeyViewModel : KeyViewModelBase
{
    #region Constructors

    public LetterKeyViewModel(char character, IRelayCommand<char> guessLetterCommand)
    {
        this.Character = character;
        this.GuessLetterCommand = guessLetterCommand;
    }

    #endregion

    #region Properties

    public char Character { get; }

    public IRelayCommand<char> GuessLetterCommand { get; }

    #endregion
}