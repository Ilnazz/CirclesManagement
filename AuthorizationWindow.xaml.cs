using CirclesManagement.Classes;
using CirclesManagement.Components;
using CirclesManagement.Pages;
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
using System.Windows.Shapes;

namespace CirclesManagement
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();

            TBUserLogin.Focus();
        }

        private void AuthorizeUser()
        {
            if (string.IsNullOrWhiteSpace(TBUserLogin.Text)
                || string.IsNullOrWhiteSpace(PBUserPassword.Password))
            {
                Helpers.Error("Необходимо заполнить все поля для авторизации.");
                return;
            }
            User user = App.DB.Users.Local
                .FirstOrDefault(u => u.Login == TBUserLogin.Text.Trim() && u.Password == PBUserPassword.Password.Trim());
            if (user is null)
            {
                Helpers.Error("Пользователь с такими данными не найден.");
                return;
            }

            if (user.Role == App.DB.Roles.Local.First(role => role.Title == "Teacher"))
            {
                if (user.Teacher.IsWorking == false)
                {
                    Helpers.Error("Учитель с такими данными был уволен.");
                    return;
                }
            }

            App.CurrentUser = user;

            var mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.Activate();
            Close();
        }

        private void BtnAuthorize_Click(object sender, RoutedEventArgs e)
            => AuthorizeUser();

        private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AuthorizeUser();
        }
    }
}
