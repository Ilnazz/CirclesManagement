using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CirclesManagement.Components
{
    public static class Helpers
    {
        private static readonly Regex regexRussianLetters = new Regex(@"[а-я]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly string questionMessageBoxCaption = "Подтверждение";

        public static bool AskQuestion(string message)
        {
            var result = MessageBox.Show(message,
                    questionMessageBoxCaption,
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
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static bool ContainsOnlyRussianLetters(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return regexRussianLetters.IsMatch(Regex.Replace(s, @"\s", ""));
        }
    }
}
