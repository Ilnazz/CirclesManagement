using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для TeachersPage.xaml
    /// </summary>
    public partial class TeachersPage : EntityPage
    {
        public TeachersPage()
        {
            InitializeComponent();

            App.DB.Teachers.Load();
            ItemsSource = new ObservableCollection<object>(App.DB.Teachers.Local);

            EH = new EntityHelper
            {
                Title = new EntityHelper.Word()
                {
                    Singular = new EntityHelper.WordCases()
                    {
                        Nominative = "учитель",
                        Genitive = "учителя",
                        Dative = "учителю",
                        Accusative = "учителя",
                        Ablative = "учителем",
                        Prepositional = "учителе"
                    },
                    Plural = new EntityHelper.WordCases()
                    {
                        Nominative = "учителя",
                        Genitive = "учителей",
                        Dative = "учителям",
                        Accusative = "учителей",
                        Ablative = "учителямм",
                        Prepositional = "учителях"
                    }
                },

                Builder = () =>
                {
                    var newTeacher = new Teacher();
                    newTeacher.LastName = "";
                    newTeacher.FirstName = "";
                    newTeacher.Patronymic = "";
                    newTeacher.IsWorking = true;
                    
                    var newUser = new User();
                    newUser.Name = "";
                    newUser.Teacher = newTeacher;
                    newUser.Login = "";
                    newUser.Password = "";
                    newUser.Role = App.DB.Roles.First(role => role.Title == "Teacher");

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

                IsDeleted = (obj) =>
                {
                    var teacher = obj as Teacher;
                    return teacher.IsWorking == false;
                },

                Comparer = (obj1, obj2) =>
                {
                    var teacher1 = obj1 as Teacher;
                    var teacher2 = obj2 as Teacher;
                    return (teacher1.LastName == teacher2.LastName
                        && teacher1.FirstName == teacher2.FirstName
                        && teacher1.Patronymic == teacher2.Patronymic)
                        || (teacher1.Login == teacher2.Login);
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

                Validator = (obj) =>
                {
                    var teacher = obj as Teacher;
                    if (string.IsNullOrWhiteSpace(teacher.LastName))
                        return (false, "фамилия не может быть пустой");
                    else if (string.IsNullOrWhiteSpace(teacher.FirstName))
                        return (false, "имя не может быть пустым");
                    else if (string.IsNullOrWhiteSpace(teacher.Patronymic))
                        return (false, "отчество не может быть пустым");
                    else if (string.IsNullOrWhiteSpace(teacher.Login))
                        return (false, "логин не может быть пустым");
                    else if (string.IsNullOrWhiteSpace(teacher.Password))
                        return (false, "пароль не может быть пустым");
                    return (true, "");
                },

                Deleter = (obj) =>
                {
                    var teacher = obj as Teacher;

                    var isPresentInTimetable = teacher.Timetables.Count > 0;
                    if (isPresentInTimetable)
                        return (false, "указан в расписании занятий");
                    
                    teacher.IsWorking = false;
                    return (true, "");
                },

                SavePreparator = (obj) =>
                {
                    var teacher = obj as Teacher;

                    teacher.Users.First().Name = teacher.FullName;
                }
            };
        }
    }
}
