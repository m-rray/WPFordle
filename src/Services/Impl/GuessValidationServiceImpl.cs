namespace WPFordle.Services.Impl;

using Models;
using Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GuessValidationServiceImpl : IGuessValidationService
{
    #region Methods

    public async Task ValidateGuessAsync(WordModel guess, WordModel target)
    {
        // TODO: This really need reworking totally, ive had to temp hack some new logic in here as i found a bug
        (LetterModel, LetterResult)[] letterResults = new (LetterModel, LetterResult)[guess.Letters.Count];
        
        List<LetterModel> matchedGuessLetters = new();
        List<LetterModel> matchedTargetLetters = new();

        // Get all correct letters first
        for (int i = 0; i < guess.Letters.Count; i++)
        {
            LetterModel letter = guess.Letters.ElementAt(i);
            LetterModel targetLetter = target.Letters.ElementAt(i);

            if (letter.Character == targetLetter.Character)
            {
                letterResults[i] = (letter, LetterResult.RightLetterRightPlace);
                matchedGuessLetters.Add(letter);
                matchedTargetLetters.Add(targetLetter);
            }
        }
        
        // Now get partially correct
        for (int i = 0; i < guess.Letters.Count; i++)
        {
            LetterModel letter = guess.Letters.ElementAt(i);
            if (matchedGuessLetters.Contains(letter))
            {
                continue;
            }

            LetterModel? partialMatch = target.Letters.Except(matchedTargetLetters).FirstOrDefault(x => x.Character == letter.Character);
            if (partialMatch != null)
            {
                letterResults[i] = (letter, LetterResult.RightLetterWrongPlace);
                matchedGuessLetters.Add(letter);
                matchedTargetLetters.Add(partialMatch);
            }
            else
            {
                letterResults[i] = (letter, LetterResult.WrongLetter);
            }
        }

        foreach ((LetterModel, LetterResult) letterResult in letterResults)
        {
            letterResult.Item1.Result = letterResult.Item2;

            if (letterResult != letterResults.Last())
            {
                await Task.Delay(Constants.TimeBetweenReveals);
            }
        }

        //for (int i = 0; i < guess.Letters.Count; i++)
        //{
        //    LetterModel letter = guess.Letters.ElementAt(i);

        //    LetterResult letterResult = LetterResult.WrongLetter;
        //    for (int j = 0; j < target.Letters.Count; j++)
        //    {
        //        LetterModel targetLetter = target.Letters.ElementAt(j);
        //        if (letter.Character == targetLetter.Character)
        //        {
        //            if (i == j)
        //            {
        //                letterResult = LetterResult.RightLetterRightPlace;
        //                break;
        //            }

        //            letterResult = LetterResult.RightLetterWrongPlace;
        //        }
        //    }

        //    letter.Result = letterResult;

        //    if (i < guess.Letters.Count - 1)
        //    {
        //        await Task.Delay(Constants.TimeBetweenReveals);
        //    }
        //}
    }

    #endregion
}