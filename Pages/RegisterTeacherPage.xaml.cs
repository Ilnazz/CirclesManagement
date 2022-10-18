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
using CirclesManagement.ADO;

namespace CirclesManagement.Pages
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

        private string NormalizeTeacherData(string raw)
        {
            raw = raw.Trim().ToLower();
            return char.ToUpper(raw[0]) + raw.Substring(1);
        }

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            Regex regexCyrillic = new Regex(@"^[\p{IsCyrillic}]+$", RegexOptions.Compiled);
            if (TBTeacherLastName.Text == ""
                || TBTeacherFirstName.Text == ""
                || TBTeacherPatronymic.Text == ""
                || TBUserLogin.Text == ""
                || PBUserPassword.Password == "")
            {
                MessageBox.Show("Для регистрации необходимо заполнить все поля данными.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else if (!regexCyrillic.IsMatch(TBTeacherLastName.Text)
                || !regexCyrillic.IsMatch(TBTeacherFirstName.Text)
                || !regexCyrillic.IsMatch(TBTeacherPatronymic.Text))
            {
                MessageBox.Show("ФИО учителя должно использовать только кириллические буквы.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else if (PBUserPassword.Password != PBUserPasswordConfirmation.Password)
            {
                MessageBox.Show("Пароли должны совпадать.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            TBTeacherLastName.Text = NormalizeTeacherData(TBTeacherLastName.Text);
            TBTeacherFirstName.Text = NormalizeTeacherData(TBTeacherFirstName.Text);
            TBTeacherPatronymic.Text = NormalizeTeacherData(TBTeacherPatronymic.Text);

            bool isTeacherExist = MainWindow.db.Teachers.ToList()
                .Exists(t =>
                    t.LastName == TBTeacherLastName.Text
                    && t.FirstName == TBTeacherFirstName.Text
                    && t.Patronymic == TBTeacherPatronymic.Text);

            if (isTeacherExist)
            {
                MessageBox.Show("Учитель с такими данными уже зарегистрирован в системе.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Teacher newTeacher = new Teacher();
            newTeacher.LastName = TBTeacherLastName.Text.Trim();
            newTeacher.FirstName = TBTeacherFirstName.Text.Trim();
            newTeacher.Patronymic = TBTeacherPatronymic.Text.Trim();

            User newUser = new User();
            newUser.RoleID = (int)Constants.Role.Teacher;
            newUser.Name = $"{TBTeacherLastName.Text} {TBTeacherFirstName.Text[0]}. {TBTeacherPatronymic.Text[0]}.";
            newUser.Login = TBUserLogin.Text;
            newUser.Password = PBUserPassword.Password;

            MainWindow.db.Teachers.Add(newTeacher);
            MainWindow.db.Users.Add(newUser);
            MainWindow.db.SaveChanges();

            MessageBox.Show("Учитель успешно зарегистрирован в системе", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }
    }
}
