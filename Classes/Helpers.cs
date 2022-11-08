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
        {
            return _regexRussianLetters.IsMatch(Regex.Replace(s, @"\s", ""));
        }

        private static readonly string _questionMessageBoxCaption = "Подтверждение";
        public static bool AskQuestion(string message)
        {
            var result = MessageBox.Show(message,
                    _questionMessageBoxCaption,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        //public static bool AskQuestionAndDoActionIfYes(string message, Action action)
        //{
        //    var result = AskQuestion(message);
        //    if (result == true)
        //        action();
        //    return result;
        //}

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
