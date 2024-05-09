using System;
using System.Globalization;
using Xamarin.Forms;

namespace coursework
{
    public class LocalPriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is either null or zero
            if (value == null || (value is double priority && Math.Abs(priority) < 0.0001)) // Consider values very close to zero as zero
            {
                return string.Empty; // Return empty string if priority is null or effectively 0
            }

            // If the value is a valid double and not zero, format it as a percentage
            return ((double)value).ToString("P2", culture); // Format as percentage with two decimal places, considering culture
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            // ConvertBack is not implemented because going from string to double isn't required in this use-case
        }
    }
}