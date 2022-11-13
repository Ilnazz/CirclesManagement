using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CirclesManagement.Classes
{
    public static class Navigation
    {
        public static bool IsUserAuthorized = false;
        public static Button BtnLogOut;
        public static Frame NavigationFrame;
        private static List<Page> _history = new List<Page>();

        public static void Next(Page page)
        {
            _history.Add(page);
            Update(_history.Last());
        }

        public static void Back()
        {
            if (_history.Count > 1)
            {
                _history.RemoveAt(_history.Count - 1);
                Update(_history.Last());
            }
        }
        
        private static void Update(Page page)
        {
            BtnLogOut.Visibility = IsUserAuthorized ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            NavigationFrame.Navigate(page);
        }
    }
}
