namespace final_project.Converters
{
    public class TimeRangeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is DateTime startTime && values[1] is DateTime endTime)
            {
                string startTimeFormatted = startTime.ToString("HH:mm");
                string endTimeFormatted = endTime.ToString("HH:mm");

                return $"{startTimeFormatted} - {endTimeFormatted}";
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}