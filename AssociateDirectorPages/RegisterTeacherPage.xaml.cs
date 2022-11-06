using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CirclesManagement.AssociateDirectorPages
{
    /// <summary>
    /// Логика взаимодействия для RegisterTeacherPage.xaml
    /// </summary>
    public partial class RegisterTeacherPage : Page
    {
        public RegisterTeacherPage()
        {
            InitializeComponent();
        }

        private void BRegister_Click(object sender, RoutedEventArgs e)
        {
            TBTeacherLastName.Text = TBTeacherLastName.Text.Trim();
            TBTeacherFirstName.Text = TBTeacherFirstName.Text.Trim();
            TBTeacherPatronymic.Text = TBTeacherPatronymic.Text.Trim();
            TBUserLogin.Text = TBUserLogin.Text.Trim();
            PBUserPassword.Password = PBUserPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(TBTeacherLastName.Text)
                || string.IsNullOrWhiteSpace(TBTeacherFirstName.Text)
                || string.IsNullOrWhiteSpace(TBTeacherPatronymic.Text)
                || string.IsNullOrWhiteSpace(TBUserLogin.Text)
                || string.IsNullOrWhiteSpace(PBUserPassword.Password))
            {
                MainWindow.StatusBar.Warning("Для регистрации необходимо заполнить все поля.");
                return;
            }
            else if (!Helpers.ContainsOnlyRussianLetters(TBTeacherLastName.Text)
                || !Helpers.ContainsOnlyRussianLetters(TBTeacherFirstName.Text)
                || !Helpers.ContainsOnlyRussianLetters(TBTeacherPatronymic.Text))
            {
                MainWindow.StatusBar.Warning("ФИО учителя должно состоять только из русских букв.");
                return;
            }
            else if (PBUserPassword.Password != PBUserPasswordConfirmation.Password)
            {
                MainWindow.StatusBar.Warning("Пароли должны совпадать.");
                return;
            }

            TBTeacherLastName.Text = Helpers.Capitalize(TBTeacherLastName.Text.ToLower());
            TBTeacherFirstName.Text = Helpers.Capitalize(TBTeacherFirstName.Text.ToLower());
            TBTeacherPatronymic.Text = Helpers.Capitalize(TBTeacherPatronymic.Text.ToLower());

            bool isTeacherExist = MainWindow.db.Teachers.ToList()
                .Exists(t =>
                    t.LastName == TBTeacherLastName.Text
                    && t.FirstName == TBTeacherFirstName.Text
                    && t.Patronymic == TBTeacherPatronymic.Text);

            if (isTeacherExist)
            {
                MainWindow.StatusBar.Error("Учитель с такими данными уже зарегистрирован в системе.");
                return;
            }

            Teacher newTeacher = new Teacher();
            newTeacher.LastName = TBTeacherLastName.Text;
            newTeacher.FirstName = TBTeacherFirstName.Text;
            newTeacher.Patronymic = TBTeacherPatronymic.Text;

            User newUser = new User();
            newUser.RoleID = (int)Constants.Role.Teacher;
            newUser.Name = $"{TBTeacherLastName.Text} {TBTeacherFirstName.Text[0]}. {TBTeacherPatronymic.Text[0]}.";
            newUser.Login = TBUserLogin.Text;
            newUser.Password = PBUserPassword.Password;

            MainWindow.db.Teachers.Add(newTeacher);
            MainWindow.db.Users.Add(newUser);
            MainWindow.db.SaveChanges();

            MainWindow.StatusBar.Info("Учитель успешно зарегистрирован в системе");
            
            Navigation.Back();
        }
    }
}
