using CirclesManagement.Classes;
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
    /// Логика взаимодействия для ClassroomsPage.xaml
    /// </summary>
    public partial class ClassroomsPage : EntityPage
    {
        public ClassroomsPage()
        {
            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newClassroom = new Classroom
                    {
                        Number = 0,
                        Title = "",
                        IsActive = true
                    };
                    App.DB.Classrooms.Add(newClassroom);
                    return newClassroom;
                },

                IsBlank = (obj) =>
                {
                    var classroom = obj as Classroom;
                    return classroom.Number == 0 && classroom.Title == "";
                },

                Validator = (obj) =>
                {
                    var classroom = obj as Classroom;
                    if (classroom.Number <= 0)
                        return (false, "номер кабинета должен быть больше нуля");
                    else if (string.IsNullOrWhiteSpace(classroom.Title))
                        return (false, "название кабинета не может быть пустым");
                    return (true, "");
                },

                Comparer = (obj1, obj2) =>
                {
                    var classroom1 = obj1 as Classroom;
                    var classroom2 = obj2 as Classroom;

                    if (classroom1.Number == classroom2.Number)
                        return (false, $"кабинет с номером {classroom1.Number} уже существует");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var classroom = obj as Classroom;
                    return $"{classroom.Number}".Contains(searchText)
                        || classroom.Title.Contains(searchText);
                },

                Deleter = (obj) =>
                {
                    var classroom = obj as Classroom;

                    var isPresentInTimetable = classroom.Timetables.Count > 0;
                    if (isPresentInTimetable)
                        return (false, $"кабинет \"{classroom.Number}/{classroom.Title}\" указан в расписании занятий");

                    classroom.IsActive = false;
                    return (true, "");
                },
                
                IsDeleted = (obj) =>
                {
                    var classroom = obj as Classroom;
                    return classroom.IsActive == false;
                },

                SavePreparator = (obj) =>
                {
                    var classroom = obj as Classroom;
                    classroom.Title = Helpers.Capitalize(classroom.Title);
                }
            };

            EntitiesSource = new ObservableCollection<object>(App.DB.Classrooms.Local);
        }
    }
}
