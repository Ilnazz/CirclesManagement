using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CirclesManagement.Classes
{
    public class Navigation
    {
        public static bool IsUserAuthorized = false;
        public static MainWindow AppWindow;
        public static List<(string, Page)> History = new List<(string, Page)>();

        public static void Next((string name, Page page) pageAndName)
        {
            History.Add(pageAndName);
            Update(History.Last());
        }

        public static void Back()
        {
            if (History.Count > 1)
            {
                History.RemoveAt(History.Count - 1);
                Update(History.Last());
            }
        }

        private static void Update((string name, Page page) pageAndName)
        {
            if (IsUserAuthorized)
                AppWindow.Title = $"Текущий пользователь: {MainWindow.CurrentUser.Name}. {pageAndName.name}";
            else
                AppWindow.Title = pageAndName.name;
            AppWindow.BtnGoToPreviousPage.Visibility = History.Count > 1 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            AppWindow.BtnLogOut.Visibility = IsUserAuthorized ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            AppWindow.MainFrame.Navigate(pageAndName.page);
        }
    }
}
