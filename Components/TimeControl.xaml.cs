using CirclesManagement.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CirclesManagement.Components
{
    /// <summary>
    /// Логика взаимодействия для TimeControl.xaml
    /// </summary>
    public partial class TimeControl : UserControl
    {
        public TimeControl()
        {
            InitializeComponent();
        }

        public new void Focus()
        {
            TBHours.Focus();
        }

        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimeControl), new PropertyMetadata(DateTime.Now.TimeOfDay, OnValueChanged));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimeControl control = obj as TimeControl;
            control.Hours = ((TimeSpan)e.NewValue).Hours;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.Seconds = ((TimeSpan)e.NewValue).Seconds;
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(TimeControl), new PropertyMetadata(0));

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(TimeControl), new PropertyMetadata(0));

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }
        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register("Seconds", typeof(int), typeof(TimeControl), new PropertyMetadata(0));

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            var timePartName = (sender as TextBox).Name;
            switch (timePartName)
            {
                case "TBSeconds":
                    if (args.Key == Key.Up)
                        Seconds++;
                    if (args.Key == Key.Down)
                        Seconds--;
                    break;

                case "TBMinutes":
                    if (args.Key == Key.Up)
                        Minutes++;
                    if (args.Key == Key.Down)
                        Minutes--;
                    break;

                case "TBHours":
                    if (args.Key == Key.Up)
                        Hours++;
                    if (args.Key == Key.Down)
                        Hours--;
                    break;
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex integerRegex = new Regex(@"\d+", RegexOptions.Compiled);
            if (integerRegex.IsMatch(e.Text) == false)
            {
                e.Handled = true;
                return;
            }
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            var TB = sender as TextBox;
            if (TB.Text == "0")
                TB.Text = "";
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var TB = sender as TextBox;
            if (string.IsNullOrWhiteSpace(TB.Text) || TB.Text == "0" || TB.Text == "00")
            {
                TB.Text = "0";
                return;
            }
            var number = int.Parse(TB.Text);
            if ((TB.Name == "TBHours" && number > 24)
                || (TB.Name == "TBMinutes" && number > 60)
                || (TB.Name == "TBSeconds" && number > 60))
                TB.Text = "0";
        }
    }

    public class TimePartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int number))
                return null;
            return number.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string numberStr))
                return 0;
            if (int.TryParse(numberStr, out int number))
                return number;
            return 0;
        }
    }
}
