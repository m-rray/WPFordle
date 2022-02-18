namespace WPFordle.Views.Converters;

using Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

public class LetterResultToBackgroundColorConverter : IValueConverter
{
    #region Properties

    public Color RightLetterRightPlaceColor { get; set; }

    public Color RightLetterWrongPlaceColor { get; set; }

    public Color WrongLetter { get; set; }

    #endregion

    #region Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not LetterResult letterResult)
        {
            return Colors.Transparent;
        }

        return letterResult switch
        {
            LetterResult.None => Colors.Transparent,
            LetterResult.RightLetterRightPlace => this.RightLetterRightPlaceColor,
            LetterResult.RightLetterWrongPlace => this.RightLetterWrongPlaceColor,
            LetterResult.WrongLetter => this.WrongLetter,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion
}