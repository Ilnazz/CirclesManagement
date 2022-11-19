using CirclesManagement.Classes;
using CirclesManagement.Components;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private User CreateNewUser(string lastName, string firstName, string patronymic,
            string login, string password)
        {
            User newUser = new User();
            newUser.RoleID = App.DB.Roles.Where(role => role.Title == "Учитель").First().ID;
            newUser.Name = $"{lastName} {firstName[0]}. {patronymic[0]}.";
            newUser.Login = login;
            newUser.Password = password;
            newUser.Teacher = CreateNewTeacher(TBLastName.Text, TBFirstName.Text, TBPatronymic.Text);
            App.DB.Users.Add(newUser);
            return newUser;
        }

        private Teacher CreateNewTeacher(string lastName, string firstName, string patronymic)
        {
            Teacher newTeacher = new Teacher();
            newTeacher.LastName = lastName;
            newTeacher.FirstName = firstName;
            newTeacher.Patronymic = patronymic;
            App.DB.Teachers.Add(newTeacher);
            return newTeacher;
        }

        private bool IsTeacherExist(string lastName, string firstName, string patronymic)
        {
            return App.DB.Teachers.ToList()
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
                Helpers.Error("Для регистрации необходимо заполнить все поля.");
                return false;
            }
            else if (PBUserPassword.Password != PBUserPasswordConfirmation.Password)
            {
                Helpers.Error("Пароли должны совпадать.");
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

            if (IsTeacherExist(TBLastName.Text, TBFirstName.Text, TBPatronymic.Text))
            {
                Helpers.Error("Учитель с такими данными уже зарегистрирован в системе.");
                return;
            }

            CreateNewUser(TBLastName.Text, TBFirstName.Text, TBPatronymic.Text,
                TBUserLogin.Text, PBUserPassword.Password);

            App.DB.SaveChanges();

            Helpers.Inform("Учитель успешно зарегистрирован в системе.");
            Close();
        }
    }
}
