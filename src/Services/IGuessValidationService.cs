namespace WPFordle.Services;

using Models;
using System.Threading.Tasks;

public interface IGuessValidationService
{
    #region Methods

    Task ValidateGuessAsync(WordModel guess, WordModel target);

    #endregion
}