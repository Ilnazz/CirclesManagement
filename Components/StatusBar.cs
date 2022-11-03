using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CirclesManagement.Components
{
    public static class StatusBar
    {
        public static TextBlock TBHeader;
        public static TextBlock TBText;

        public enum StatusMessageType
        {
            Information, Warning, Error
        }

        private static void Update(string message, StatusMessageType type)
        {
            switch (type)
            {
                case StatusMessageType.Information:
                    TBText.Foreground = Brushes.Black;
                    break;
                case StatusMessageType.Warning:
                    TBText.Foreground = Brushes.Orange;
                    break;
                case StatusMessageType.Error:
                    TBText.Foreground = Brushes.Red;
                    break;
            }
            TBHeader.Text = $"[{DateTime.Now.ToLongTimeString()}]";
            TBText.Text = message;
        }

        public static void Info(string message) => Update(message, StatusMessageType.Information);
        public static void Warning(string message) => Update(message, StatusMessageType.Warning);
        public static void Error(string message) => Update(message, StatusMessageType.Error);
    }
}
