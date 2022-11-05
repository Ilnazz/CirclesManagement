using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для StatusBarComponent.xaml
    /// </summary>
    public partial class StatusBarComponent : UserControl
    {
        private enum StatusMessageType
        {
            Information, Warning, Error
        }

        public StatusBarComponent()
        {
            InitializeComponent();
        }
        
        private void Update(string message, StatusMessageType type)
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
            TBHeader.Text = $"[{DateTime.Now.ToLongTimeString()}]:";
            TBText.Text = message;
        }

        public void Info(string message) => Update(message, StatusMessageType.Information);
        public void Warning(string message) => Update(message, StatusMessageType.Warning);
        public void Error(string message) => Update(message, StatusMessageType.Error);
        
        public void Clear() => TBHeader.Text = TBText.Text = "";
    }
}
