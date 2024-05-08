using System;
using System.Globalization;
using Xamarin.Forms;

namespace coursework
{
    //used for displaying empty string if priority is 0 and converts the LocalPriority to percentage if not 0 and displays it in the list
    public class LocalPriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double priority && priority == 0)
            {
                return string.Empty; // Return empty string if priority is 0
            }
            return ((double)value).ToString("P2"); // Otherwise, format as percentage
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}