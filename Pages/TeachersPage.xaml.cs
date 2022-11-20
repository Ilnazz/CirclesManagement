using CirclesManagement.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для TeachersPage.xaml
    /// </summary>
    public partial class TeachersPage : EntityPage
    {
        public TeachersPage()
        {
            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newTeacher = new Teacher
                    {
                        LastName = "",
                        FirstName = "",
                        Patronymic = "",
                        IsWorking = true
                    };

                    var newUser = new User
                    {
                        Name = "",
                        Teacher = newTeacher,
                        Login = "",
                        Password = "",
                        Role = App.DB.Roles.First(role => role.Title == "Teacher")
                    };

                    App.DB.Users.Add(newUser);
                    App.DB.Teachers.Local.Add(newTeacher);
                    return newTeacher;
                },

                IsBlank = (obj) =>
                {
                    var teacher = obj as Teacher;
                    return teacher.LastName == ""
                        && teacher.FirstName == ""
                        && teacher.Patronymic == ""
                        && teacher.Login == ""
                        && teacher.Password == "";
                },

                Validator = (obj) =>
                {
                    var teacher = obj as Teacher;
                    if (string.IsNullOrWhiteSpace(teacher.LastName))
                        return (false, "фамилия учителя не может быть пустой");
                    else if (string.IsNullOrWhiteSpace(teacher.FirstName))
                        return (false, "имя учителя не может быть пустым");
                    else if (string.IsNullOrWhiteSpace(teacher.Patronymic))
                        return (false, "отчество учителя не может быть пустым");
                    else if (string.IsNullOrWhiteSpace(teacher.Login))
                        return (false, "логин учителя не может быть пустым");
                    else if (string.IsNullOrWhiteSpace(teacher.Password))
                        return (false, "пароль учителя не может быть пустым");
                    return (true, "");
                },

                Comparer = (obj1, obj2) =>
                {
                    var teacher1 = obj1 as Teacher;
                    var teacher2 = obj2 as Teacher;
                    if (teacher1.LastName.ToLower().Trim() == teacher2.LastName.ToLower().Trim()
                        && teacher1.FirstName.ToLower().Trim() == teacher2.FirstName.ToLower().Trim()
                        && teacher1.Patronymic.ToLower().Trim() == teacher2.Patronymic.ToLower().Trim())
                            return (false, $"учитель с ФИО {teacher1.LastName} {teacher1.FirstName} {teacher1.Patronymic} уже существует");
                    if (teacher1.Login == teacher2.Login)
                        return (false, $"логин учителя должен быть уникальным, {teacher1.Login} - уже существует");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var teacher = obj as Teacher;
                    return teacher.LastName.Contains(searchText)
                        || teacher.FirstName.Contains(searchText)
                        || teacher.Patronymic.Contains(searchText)
                        || teacher.Login.Contains(searchText)
                        || teacher.Password.Contains(searchText);
                },

                Deleter = (obj) =>
                {
                    var teacher = obj as Teacher;

                    var isPresentInTimetable = teacher.Timetables.Count > 0;
                    if (isPresentInTimetable)
                        return (false, $"у учителя \"{teacher.FullName}\" есть занятие по расписанию");

                    teacher.IsWorking = false;
                    return (true, "");
                },

                IsDeleted = (obj) =>
                {
                    var teacher = obj as Teacher;
                    return teacher.IsWorking == false;
                },

                SavePreparator = (obj) =>
                {
                    var teacher = obj as Teacher;

                    teacher.Users.First().Name = teacher.FullName;
                }
            };

            EntitiesSource = new ObservableCollection<object>(App.DB.Teachers.Local);
        }
    }
}
