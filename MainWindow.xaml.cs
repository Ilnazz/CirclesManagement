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
        public static ApplicationSetting ApplicationSetting { get; private set; }
        public static StatusBarComponent StatusBar { get; private set; }
        public static User CurrentUser;

        public MainWindow()
        {
            InitializeComponent();

            db = new CirclesManagementEntities();

            if (db.ApplicationSettings.Count() == 0) // application starts for the first time
            {
                ApplicationSetting = new ApplicationSetting();
                ApplicationSetting.GradeNumerationTypeID = (int)Constants.GradeNumerationType.NumberAndLetter;
            }
            else
                ApplicationSetting = db.ApplicationSettings.Single();
            db.SaveChanges();

            StatusBar = StatusBarComponentInstance;

            Navigation.AppWindow = this;

            if (db.Users.Count() == 0)
                Navigation.Next(("Страница регистрации пользователя", new RegistrationPage(Constants.Role.AssociateDirector)));
            else
                Navigation.Next(("Страница авторизации", new AuthPage()));
        }

        private void BtnGoToPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Back();
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var result = Helpers.AskQuestion("Вы уверены, что хотите выйти из системы?");
            if (result == true)
            {
                CurrentUser = null;
                Navigation.IsUserAuthorized = false;
                Navigation.History.Clear();
                Navigation.Next(("Страница авторизации", new AuthPage()));
                StatusBar.Info("Вы успешно вышли из системы.");
            }
        }
    }
}
