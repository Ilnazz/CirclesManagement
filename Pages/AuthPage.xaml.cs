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
            TBUserLogin.Focus();
        }

        private void Button_Authorize_Click(object sender, RoutedEventArgs e)
        {
            if (TBUserLogin.Text == ""
                || PBUserPassword.Password == "")
            {
                MessageBox.Show("Необходимо заполнить все данные для авторизации.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            User user = MainWindow.db.Users.ToList()
                .FirstOrDefault(u => u.Login == TBUserLogin.Text.Trim() && u.Password == PBUserPassword.Password.Trim());
            if (user is null)
            {
                MessageBox.Show("Проверьте данные. Пользователь не найден.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Авторизация прошла успешно.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            
            MainWindow.CurrentUser = user;
            Navigation.IsUserAuthorized = true;
            Navigation.History.Clear();

            if (user.RoleID == (int)Constants.Role.AssociateDirector)
                Navigation.Next(("Главная страница зам. директора по воспитательной работе", new AssociateDirectorMainPage()));
            else
                Navigation.Next(("Главная странциа учителя", new TeacherMainPage()));
        }
    }
}
