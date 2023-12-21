using CirclesManagement.Pages;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CirclesManagement.Converters
{
    public class ReportTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReportType rp == false)
                return null;

            if (rp == ReportType.PupilCountByGroup)
                return "Количество учеников по группам";

            else if (rp == ReportType.LessonCountByWeekDay)
                return "Общее количество занятий по дням недели";

            else if (rp == ReportType.CirclesAttendance)
                return "Процент посещаемости";

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}