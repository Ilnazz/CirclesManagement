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

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterTeacherPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private Constants.Role _newUserRole;

        public RegistrationPage(Constants.Role newUserRole)
        {
            InitializeComponent();
            _newUserRole = newUserRole;
        }

        private User CreateNewUser(string lastName, string firstName, string patronymic,
            string login, string password, Constants.Role role)
        {
            User newUser = new User();
            newUser.RoleID = (int)role;
            newUser.Name = $"{lastName} {firstName[0]}. {patronymic[0]}.";
            newUser.Login = login;
            newUser.Password = password;
            newUser.TeacherID = null;
            MainWindow.db.Users.Add(newUser);
            return newUser;
        }

        private Teacher CreateNewTeacher(string lastName, string firstName, string patronymic)
        {
            Teacher newTeacher = new Teacher();
            newTeacher.LastName = lastName;
            newTeacher.FirstName = firstName;
            newTeacher.Patronymic = patronymic;
            MainWindow.db.Teachers.Add(newTeacher);
            return newTeacher;
        }

        private bool IsTeacherExist(string lastName, string firstName, string patronymic)
        {
            return MainWindow.db.Teachers.ToList()
                    .Exists(t =>
                        t.LastName == TBLastName.Text
                        && t.FirstName == TBFirstName.Text
                        && t.Patronymic == TBPatronymic.Text);
        }

        private bool AreFieldsFilledCorrectly()
        {
            if (string.IsNullOrWhiteSpace(TBLastName.Text)
                || string.IsNullOrWhiteSpace(TBFirstName.Text)
                || string.IsNullOrWhiteSpace(TBPatronymic.Text)
                || string.IsNullOrWhiteSpace(TBUserLogin.Text)
                || string.IsNullOrWhiteSpace(PBUserPassword.Password))
            {
                MainWindow.StatusBar.Warning("Для регистрации необходимо заполнить все поля.");
                return false;
            }
            else if (!Helpers.ContainsOnlyRussianLetters(TBLastName.Text)
                || !Helpers.ContainsOnlyRussianLetters(TBFirstName.Text)
                || !Helpers.ContainsOnlyRussianLetters(TBPatronymic.Text))
            {
                MainWindow.StatusBar.Warning("ФИО должно состоять только из русских букв.");
                return false;
            }
            else if (PBUserPassword.Password != PBUserPasswordConfirmation.Password)
            {
                MainWindow.StatusBar.Warning("Пароли должны совпадать.");
                return false;
            }
            return true;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            TBLastName.Text = TBLastName.Text.Trim();
            TBFirstName.Text = TBFirstName.Text.Trim();
            TBPatronymic.Text = TBPatronymic.Text.Trim();
            TBUserLogin.Text = TBUserLogin.Text.Trim();
            PBUserPassword.Password = PBUserPassword.Password.Trim();

            if (AreFieldsFilledCorrectly() == false)
                return;

            TBLastName.Text = Helpers.Capitalize(TBLastName.Text.ToLower());
            TBFirstName.Text = Helpers.Capitalize(TBFirstName.Text.ToLower());
            TBPatronymic.Text = Helpers.Capitalize(TBPatronymic.Text.ToLower());

            if (_newUserRole == Constants.Role.Teacher
                && IsTeacherExist(TBLastName.Text, TBFirstName.Text, TBPatronymic.Text))
            {
                MainWindow.StatusBar.Error("Учитель с такими данными уже зарегистрирован в системе.");
                return;
            }

            var newUser = CreateNewUser(TBLastName.Text, TBFirstName.Text, TBPatronymic.Text,
                TBUserLogin.Text, PBUserPassword.Password, _newUserRole);

            if (_newUserRole == Constants.Role.Teacher)
            {
                // think about ID's !
                newUser.Teacher = CreateNewTeacher(TBLastName.Text, TBFirstName.Text, TBPatronymic.Text);
            }

            MainWindow.db.SaveChanges();

            MainWindow.StatusBar.Info("Учитель успешно зарегистрирован в системе");

            if (_newUserRole == Constants.Role.Teacher)
                Navigation.Next(("Учитель", new TeacherPages.MainPage()));
            else
                Navigation.Next(("Зам. директора по воспитательной работе", new AssociateDirectorPages.MainPage()));
        }
    }
}
