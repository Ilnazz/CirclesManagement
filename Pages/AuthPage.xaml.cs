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

using CirclesManagement.Classes;
using CirclesManagement.Components;

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthorizeUser()
        {
            if (string.IsNullOrWhiteSpace(TBUserLogin.Text)
                || string.IsNullOrWhiteSpace(PBUserPassword.Password))
            {
                MainWindow.StatusBar.Warning("Необходимо заполнить все поля для авторизации.");
                return;
            }
            User user = MainWindow.db.Users.ToList()
                .FirstOrDefault(u => u.Login == TBUserLogin.Text.Trim() && u.Password == PBUserPassword.Password.Trim());
            if (user is null)
            {
                MainWindow.StatusBar.Error("Пользователь с такими данными не найден.");
                return;
            }
            MainWindow.StatusBar.Info("Авторизация прошла успешно.");
            
            MainWindow.CurrentUser = user;
            Navigation.IsUserAuthorized = true;
            Navigation.History.Clear();

            if (user.RoleID == (int)Constants.Role.AssociateDirector)
                Navigation.Next(("Главная страница зам. директора по воспитательной работе", new AssociateDirectorPages.MainPage()));
            else
                Navigation.Next(("Главная странциа учителя", new TeacherPages.MainPage()));
        }

        private void BAuthorize_Click(object sender, RoutedEventArgs e)
        {
            AuthorizeUser();
        }

        private void Field_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AuthorizeUser();
        }
    }
}
