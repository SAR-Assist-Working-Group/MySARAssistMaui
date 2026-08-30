using System.Globalization;

namespace MySARAssist.Converters
{
    public class IntConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0";
            int thedecimal = (int)value;
            return thedecimal.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string ?? string.Empty;

            if (string.IsNullOrEmpty(strValue))
                return Binding.DoNothing; // Don't update source while field is being cleared

            if (int.TryParse(strValue, out int result))
                return result;

            return Binding.DoNothing;
        }

    }
}
