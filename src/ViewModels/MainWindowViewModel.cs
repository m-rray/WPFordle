namespace WPFordle.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;
using Models.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class MainWindowViewModel : ObservableObject
{
    #region Fields

    private readonly IMessageService _messageService;

    [ObservableProperty]
    private GameViewModel? _game;

    #endregion

    #region Constructors

    public MainWindowViewModel(IMessageService messageService, IThemeService themeService)
    {
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(themeService);

        this._messageService = messageService;

        this.Keyboard = new KeyboardViewModel(
            this.InputCharacterCommand,
            this.CommitGuessCommand,
            this.DeleteLastCharacterCommand);

        this.Settings = new SettingsViewModel(themeService);
    }

    #endregion

    #region Properties

    public KeyboardViewModel Keyboard { get; }

    public SettingsViewModel Settings { get; }

    #endregion

    #region Methods

    [ICommand]
    private void InputCharacter(char input)
    {
        this.Game.Model.GuessLetter(input);
    }

    [ICommand]
    private async Task CommitGuess()
    {
        GuessResult? guess = await this.Game.Model.CommitGuess();
        if (guess == null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(guess.Message))
        {
            this._messageService.FireMessage(guess.Message);
        }

        if (guess.Validated && guess.GuessedWord != null)
        {
            foreach (LetterModel guessedWordLetter in guess.GuessedWord.Letters)
            {
                this.Keyboard.UpdateLetterResult(guessedWordLetter.Character.Value, guessedWordLetter.Result);
            }
        }
    }

    [ICommand]
    private void DeleteLastCharacter()
    {
        this.Game.Model.DeleteLastCharacter();
    }

    public async Task StartNewGameAsync(IGuessValidationService guessValidationService, IWordService wordService)
    {
        string dailyWord = await wordService.GetDailyWordAsync();
        WordModel targetWord = new(dailyWord);
        GameModel gameModel = new(targetWord, guessValidationService, wordService);
        this.Game = new GameViewModel(gameModel);
    }

    #endregion
}

public class KeyboardViewModel : ObservableObject
{
    #region Fields

    private static readonly char[] KeyboardRow1 =
    {
        'Q',
        'W',
        'E',
        'R',
        'T',
        'Y',
        'U',
        'I',
        'O',
        'P'
    };

    private static readonly char[] KeyboardRow2 =
    {
        'A',
        'S',
        'D',
        'F',
        'G',
        'H',
        'J',
        'K',
        'L'
    };

    private static readonly char[] KeyboardRow3 =
    {
        'Z',
        'X',
        'C',
        'V',
        'B',
        'N',
        'M'
    };

    #endregion

    #region Constructors

    public KeyboardViewModel(
        IRelayCommand<char> guessLetterCommand,
        IRelayCommand commitGuessCommand,
        IRelayCommand deleteLastGuessCommand)
    {
        IReadOnlyCollection<KeyViewModelBase> rowOneKeys =
            KeyboardRow1.Select(x => new LetterKeyViewModel(x, guessLetterCommand)).ToList();

        IReadOnlyCollection<KeyViewModelBase> rowTwoKeys =
            KeyboardRow2.Select(x => new LetterKeyViewModel(x, guessLetterCommand)).ToList();

        List<KeyViewModelBase> rowThreeKeys =
            KeyboardRow3.Select(x => new LetterKeyViewModel(x, guessLetterCommand)).ToList<KeyViewModelBase>();
        rowThreeKeys.Insert(0, new EnterKeyViewModel(commitGuessCommand));
        rowThreeKeys.Add(new DeleteKeyViewModel(deleteLastGuessCommand));

        this.Keys = new List<IReadOnlyCollection<KeyViewModelBase>>
        {
            rowOneKeys,
            rowTwoKeys,
            rowThreeKeys
        };
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<IReadOnlyCollection<KeyViewModelBase>> Keys { get; }

    #endregion

    #region Methods

    public void UpdateLetterResult(char character, LetterResult letterResult)
    {
        LetterKeyViewModel? match = this.Keys.SelectMany(x => x)
            .OfType<LetterKeyViewModel>()
            .FirstOrDefault(x => x.Character == character);

        match?.UpdateLetterResult(letterResult);
    }

    #endregion
}

public abstract partial class KeyViewModelBase : ObservableObject
{
    #region Fields

    [ObservableProperty]
    private LetterResult _letterResult;

    #endregion
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

    #region Methods

    public void UpdateLetterResult(LetterResult letterResult)
    {
        if (letterResult is LetterResult.None or LetterResult.WrongLetter)
        {
            this.LetterResult = letterResult;
        }
        else if (letterResult == LetterResult.RightLetterWrongPlace
                 && this.LetterResult != LetterResult.RightLetterRightPlace)
        {
            this.LetterResult = letterResult;
        }
        else
        {
            this.LetterResult = letterResult;
        }
    }

    #endregion
}

public class SettingsViewModel : ObservableObject
{
    #region Fields

    private readonly IThemeService _themeService;
    private bool _darkTheme;
    private bool _highContrast;

    #endregion

    #region Constructors

    public SettingsViewModel(IThemeService themeService)
    {
        ArgumentNullException.ThrowIfNull(themeService);

        this._themeService = themeService;
        IThemeService.Theme currentTheme = themeService.GetCurrentTheme();

        switch (currentTheme)
        {
            case IThemeService.Theme.Dark:
                this._darkTheme = true;
                this._highContrast = false;
                break;
            case IThemeService.Theme.DarkHighContrast:
                this._darkTheme = true;
                this._highContrast = true;
                break;
            case IThemeService.Theme.Light:
                this._darkTheme = false;
                this._highContrast = false;
                break;
            case IThemeService.Theme.LightHighContrast:
                this._darkTheme = false;
                this._highContrast = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region Properties

    public bool DarkTheme
    {
        get => this._darkTheme;
        set
        {
            this._darkTheme = value;
            this.OnPropertyChanged();
            this._themeService.SetThemeSettings(value, this.HighContrast);
        }
    }

    public bool HighContrast
    {
        get => this._highContrast;
        set
        {
            this._highContrast = value;
            this.OnPropertyChanged();
            this._themeService.SetThemeSettings(this.DarkTheme, value);
        }
    }

    #endregion
}