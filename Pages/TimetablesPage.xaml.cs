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
    /// Логика взаимодействия для TimetablesPage.xaml
    /// </summary>
    public partial class TimetablesPage : EntityPage
    {
        public TimetablesPage()
        {
            InitializeComponent();

            App.DB.Timetables.Load();
            ItemsSource = new ObservableCollection<object>(App.DB.Timetables.Local);

            EntityCollectionsForComboBoxes = new
            {
                WeekDays = App.DB.WeekDays.Local,
                Circles = App.DB.Circles.Local,
                Classrooms = App.DB.Classrooms.Local,
                Teachers = App.DB.Teachers.Local
            };

            EH = new EntityHelper
            {
                Title = new EntityHelper.Word()
                {
                    Singular = new EntityHelper.WordCases()
                    {
                        Nominative = "запись в расписании",
                        Genitive = "запись в расписании",
                        Dative = "записи в расписании",
                        Accusative = "запись в расписании",
                        Ablative = "записью в расписании",
                        Prepositional = "записи в расписании"
                    },
                    Plural = new EntityHelper.WordCases()
                    {
                        Nominative = "записи в расписании",
                        Genitive = "записей в расписании",
                        Dative = "записям в расписании",
                        Accusative = "записи в расписании",
                        Ablative = "записями в расписании",
                        Prepositional = "записях в расписании"
                    }
                },
                
                Builder = () =>
                {
                    var newTimetable = new Timetable();
                    newTimetable.Time = TimeSpan.Zero;
                    newTimetable.IsActive = true;
                    App.DB.Timetables.Local.Add(newTimetable);
                    return newTimetable;
                },

                IsBlank = (obj) =>
                {
                    var timetable = obj as Timetable;

                    return timetable.WeekDay == null
                        && timetable.Time == TimeSpan.Zero
                        && timetable.Circle == null
                        && timetable.Classroom == null
                        && timetable.Teacher == null;
                },

                IsDeleted = (obj) =>
                {
                    var timetable = obj as Timetable;
                    return timetable.IsActive == false;
                },

                Comparer = (obj1, obj2) =>
                {
                    var timetable1 = obj1 as Timetable;
                    var timetable2 = obj2 as Timetable;

                    return timetable1.WeekDay == timetable2.WeekDay
                        && timetable1.Time == timetable2.Time
                        && timetable1.Teacher == timetable2.Teacher;
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var timetable = obj as Timetable;
                    return timetable.WeekDay.Title.Contains(searchText)
                        || timetable.Time.ToString().Contains(searchText)
                        || timetable.Circle.Title.Contains(searchText)
                        || timetable.Classroom.Title.Contains(searchText)
                        || timetable.Teacher.FullName.Contains(searchText);
                },

                Validator = (obj) =>
                {
                    var timetable = obj as Timetable;
                    // check time somehow
                    return (true, "");
                },

                Deleter = (obj) =>
                {
                    var timetable = obj as Timetable;

                    timetable.IsActive = false;
                    return (true, "");
                }
            };
        }
    }
}
