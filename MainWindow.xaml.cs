using System;
using System.Collections.Generic;
using System.Data.Entity;
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

using CirclesManagement.Pages;
using CirclesManagement.Components;

namespace CirclesManagement
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static CirclesManagementEntities db = new CirclesManagementEntities();
        public static User CurrentUser;

        public enum Role
        {
            AssociateDirector = 1, Teacher = 2
        }

        public MainWindow()
        {
            InitializeComponent();

            Navigation.AppWindow = this;

            Navigation.Next(("Страница авторизации", new AuthPage()));

            //TODO: On first running of this application it is neccessary to provide form to register associate director
        }

        private void BGoToPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Back();
        }

        private void BLogOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из системы?", "Подтверждение",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                return;
            CurrentUser = null;
            Navigation.IsUserAuthorized = false;
            Navigation.History.Clear();
            Navigation.Next(("Страница авторизации", new AuthPage()));
        }
    }
}
