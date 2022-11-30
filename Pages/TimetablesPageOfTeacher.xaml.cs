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
    /// Логика взаимодействия для TimetablesPage.xaml
    /// </summary>
    public partial class TimetablesPageOfTeacher : EntityPage
    {
        public TimetablesPageOfTeacher(Teacher teacher)
        {
            HasAddFunction = false;
            HasDeleteFunction = false;

            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newTimetable = new Timetable
                    {
                        WeekDay = null,
                        Time = TimeSpan.Zero,
                        Classroom = null,
                        Group = null,
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
                        && timetable.Classroom == null
                        && timetable.Group == null;
                },

                Validator = (obj) =>
                {
                    var timetable = obj as Timetable;

                    if (timetable.WeekDay == null)
                        return (false, "не указан день недели");
                    else if (timetable.Time == TimeSpan.Zero)
                        return (false, "не указано время занятий");
                    else if (timetable.Group == null)
                        return (false, "не указана группа");
                    else if (timetable.Classroom == null)
                        return (false, "не указан кабинет");
                    return (true, "");
                },
                
                Comparer = (obj1, obj2) =>
                {
                    var timetable1 = obj1 as Timetable;
                    var timetable2 = obj2 as Timetable;
                    
                    if (timetable1.WeekDay == timetable2.WeekDay
                        && timetable1.Time == timetable2.Time
                        && timetable1.Group.Teacher == timetable2.Group.Teacher)
                            // Будет НЕОДНОЗНАЧНОСТЬ, ЕСЛИ Фамилия и инициалы совпадают
                            return (false, $"учитель группы \"{timetable1.Group.Teacher.FullName}\" не может быть назначен " +
                                $"дважды на тот же день недели ({timetable1.WeekDay.Title}) " +
                                $"на то же время ({timetable2.Time:hh\\:mm})");
                    return (true, "");
                },

                SearchTextMatcher = (obj, searchText) =>
                {
                    var timetable = obj as Timetable;
                    return (timetable.WeekDay != null && timetable.WeekDay.Title.Contains(searchText))
                        || timetable.Time.ToString().Contains(searchText)
                        || (timetable.Group.Circle != null && timetable.Group.Circle.Title.Contains(searchText))
                        || (timetable.Classroom != null && timetable.Classroom.Title.Contains(searchText))
                        || (timetable.Group.Teacher != null && timetable.Group.Teacher.FullName.Contains(searchText));
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

            dataGridGroupping = (
                new PropertyGroupDescription("WeekDay.Title"),
                FindResource("TimetableDataGridGroupStyle") as GroupStyle
            );

            EntitiesSource = new ObservableCollection<object>(App.DB.Timetables.Local.Where(t => t.Group.Teacher == teacher));

            DataContext = new
            {
                WeekDays = App.DB.WeekDays.Local,
                Classrooms = App.DB.Classrooms.Local.Where(classroom => classroom.IsActive == true),
                Groups = App.DB.Groups.Local.Where(group => group.IsActive == true && group.Teacher == teacher)
            };

            HasCreateLessonFunction = true;
            CreateLessonFunction += () =>
            {
                if (App.DB.ChangeTracker.HasChanges() == true)
                {
                    Helpers.Error("Перед тем, как провести урок, необходимо сохранить изменения в расписании.");
                    return;
                }

                if (InnerDataGrid.SelectedItem == null)
                    return;
                
                var selectedTimetable = InnerDataGrid.SelectedItem as Timetable;

                var newLesson = new Lesson
                {
                    Timetable = selectedTimetable,
                    Date = DateTime.Now,
                    IsConducted = false
                };

                App.DB.Lessons.Local.Add(newLesson);
                App.DB.SaveChanges();

                Helpers.Inform("Урок создан.");
            };
        }
    }
}
