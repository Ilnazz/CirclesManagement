using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CirclesManagement.Components
{
    public static class Helpers
    {
        private static readonly Regex regexRussianLetters = new Regex(@"[а-я]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
