using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CirclesManagement.Classes
{
    public static class Helpers
    {
        private static readonly Regex _regexRussianLetters = new Regex(@"^[а-яА-Я]+$", RegexOptions.Compiled);
        public static bool ContainsOnlyRussianLetters(string s)
            => _regexRussianLetters.IsMatch(Regex.Replace(s, @"\s", ""));

        private static readonly Regex _regexGradeTitle = new Regex(@"^\d{1,2}[а-яА-Я]?$");
        public static bool IsCorrectGradeTitle(string s)
            => _regexGradeTitle.IsMatch(Regex.Replace(s, @"\s", ""));

        private static readonly string _questionMessageBoxCaption = "Подтверждение";
        public static bool Ask(string message)
        {
            var result = MessageBox.Show(message,
                    _questionMessageBoxCaption,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private static readonly string _warningMessageBoxCaption = "Предупреждение";
        public static bool Warn(string message)
        {
            var result = MessageBox.Show(message,
                    _warningMessageBoxCaption,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
        }

        public static void AskAndDoActionIfYes(string message, Action action)
        {
            if (Ask(message) == true)
                action.Invoke();
        }

        public static void WarnAndDoActionIfYes(string message, Action action)
        {
            if (Warn(message) == true)
                action.Invoke();
        }

        private static readonly string _informationMessageBoxCaption = "Уведомление";
        public static void Inform(string message)
            => MessageBox.Show(message, _informationMessageBoxCaption,
                MessageBoxButton.OK, MessageBoxImage.Information);

        private static readonly string _errorMessageBoxCaption = "Ошибка";
        public static void Error(string message)
            => MessageBox.Show(message, _errorMessageBoxCaption,
                MessageBoxButton.OK, MessageBoxImage.Error);

        public static string Capitalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;
            else if (s.Length == 1)
                return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
