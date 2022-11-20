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
    /// Логика взаимодействия для TimetablesPage.xaml
    /// </summary>
    public partial class TimetablesPage : EntityPage
    {
        public TimetablesPage()
        {
            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newTimetable = new Timetable
                    {
                        WeekDay = null,
                        Time = TimeSpan.Zero,
                        Circle = null,
                        Classroom = null,
                        Teacher = null,
                        IsActive = true
                    };
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

                Validator = (obj) =>
                {
                    var timetable = obj as Timetable;

                    if (timetable.WeekDay == null)
                        return (false, "не указан день недели");
                    else if (timetable.Time == TimeSpan.Zero)
                        return (false, "не указано время занятий");
                    else if (timetable.Circle == null)
                        return (false, "не указан кружок");
                    else if (timetable.Classroom == null)
                        return (false, "не указан кабинет");
                    else if (timetable.Teacher == null)
                        return (false, "не указан учитель");
                    return (true, "");
                },
                
                Comparer = (obj1, obj2) =>
                {
                    var timetable1 = obj1 as Timetable;
                    var timetable2 = obj2 as Timetable;
                    // Будет НЕОДНОЗНАЧНОСТЬ, ЕСЛИ Фамилия и инициалы совпадают
                    if (timetable1.WeekDay == timetable2.WeekDay
                        && timetable1.Time == timetable2.Time
                        && timetable1.Teacher == timetable2.Teacher)
                            return (false, $"учитель \"{timetable1.Teacher.FullName}\" не может быть назначен " +
                                $"дважды на тот же день недели ({timetable1.WeekDay.Title}) " +
                                $"и на то же время ({timetable2.Time:hh\\:mm})");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var timetable = obj as Timetable;
                    return (timetable.WeekDay == null || timetable.WeekDay.Title.Contains(searchText))
                        || timetable.Time.ToString().Contains(searchText)
                        || (timetable.Circle == null || timetable.Circle.Title.Contains(searchText))
                        || (timetable.Classroom == null || timetable.Classroom.Title.Contains(searchText))
                        || (timetable.Teacher == null || timetable.Teacher.FullName.Contains(searchText));
                },

                Deleter = (obj) =>
                {
                    var timetable = obj as Timetable;

                    timetable.IsActive = false;
                    return (true, "");
                },

                IsDeleted = (obj) =>
                {
                    var timetable = obj as Timetable;
                    return timetable.IsActive == false;
                },
            };

            propertyGroupDescriptions = new PropertyGroupDescription[]
            {
                new PropertyGroupDescription("WeekDays.Title")
            };

            EntitiesSource = new ObservableCollection<object>(App.DB.Timetables.Local);

            DataContext = new
            {
                WeekDays = App.DB.WeekDays.Local,
                Circles = App.DB.Circles.Local,
                Classrooms = App.DB.Classrooms.Local,
                Teachers = App.DB.Teachers.Local
            };
        }
    }
}
