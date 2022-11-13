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

using CirclesManagement.Classes;
using CirclesManagement.Components;
using CirclesManagement.Pages;

namespace CirclesManagement
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static CirclesManagementEntities db { get; private set; }
        public static User CurrentUser;

        public MainWindow()
        {
            InitializeComponent();

            db = new CirclesManagementEntities();

            Navigation.NavigationFrame = MainFrame;
            Navigation.BtnLogOut = BtnLogOut;

            if (db.Users.Count() == 0)
                Navigation.Next(new RegistrationPage(Constants.Role.AssociateDirector));
            else
                Navigation.Next(new AuthorizationPage());
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Helpers.AskAndDoActionIfYes("Вы уверены, что хотите выйти из системы?", () =>
            {
                CurrentUser = null;
                Navigation.IsUserAuthorized = false;
                Navigation.Back();
                Helpers.Inform("Вы успешно вышли из системы.");
            });
        }
    }
}
