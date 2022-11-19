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
    /// Логика взаимодействия для ClassroomsPage.xaml
    /// </summary>
    public partial class ClassroomsPage : EntityPage
    {
        public ClassroomsPage()
        {
            InitializeComponent();

            App.DB.Classrooms.Load();
            ItemsSource = new ObservableCollection<object>(App.DB.Classrooms.Local);

            EH = new EntityHelper
            {
                Title = new EntityHelper.Word()
                {
                    Singular = new EntityHelper.WordCases()
                    {
                        Nominative = "кабинет",
                        Genitive = "кабинета",
                        Dative = "кабинету",
                        Accusative = "кабинет",
                        Ablative = "кабинетом",
                        Prepositional = "кабинете"
                    },
                    Plural = new EntityHelper.WordCases()
                    {
                        Nominative = "кабинеты",
                        Genitive = "кабинетов",
                        Dative = "кабинетам",
                        Accusative = "кабинеты",
                        Ablative = "кабинетами",
                        Prepositional = "кабинетах"
                    }
                },
                
                Builder = () =>
                {
                    var newClassroom = new Classroom();
                    newClassroom.Number = 0;
                    newClassroom.Title = "";
                    newClassroom.IsActive = true;
                    App.DB.Classrooms.Add(newClassroom);
                    return newClassroom;
                },

                IsBlank = (obj) =>
                {
                    var classroom = obj as Classroom;
                    return classroom.Number == 0 && classroom.Title == "";
                },

                IsDeleted = (obj) =>
                {
                    var classroom = obj as Classroom;
                    return classroom.IsActive == false;
                },

                Comparer = (obj1, obj2) =>
                {
                    var classroom1 = obj1 as Classroom;
                    var classroom2 = obj2 as Classroom;
                    return classroom1.Number == classroom2.Number
                        && classroom1.Title == classroom2.Title;
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var classroom = obj as Classroom;
                    return $"{classroom.Number}".Contains(searchText)
                        || classroom.Title.Contains(searchText);
                },

                Validator = (obj) =>
                {
                    var classroom = obj as Classroom;
                    if (classroom.Number <= 0)
                        return (false, "номер должен быть числом большим нуля");
                    else if (string.IsNullOrWhiteSpace(classroom.Title))
                        return (false, "название не может быть пустым");
                    else if (Helpers.ContainsOnlyRussianLetters(classroom.Title) == false)
                        return (false, "название должно содержать только русские буквы");
                    return (true, "");
                },

                Deleter = (obj) =>
                {
                    var classroom = obj as Classroom;

                    var isPresentInTimetable = classroom.Timetables.Count > 0;
                    if (isPresentInTimetable)
                        return (false, "указан в расписании занятий");
                    
                    classroom.IsActive = false;
                    return (true, "");
                }
            };
        }
    }
}
