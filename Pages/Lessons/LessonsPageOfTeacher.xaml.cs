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
    /// Логика взаимодействия для LessonsPage.xaml
    /// </summary>
    public partial class LessonsPageOfTeacher : EntityPage
    {
        public LessonsPageOfTeacher(Teacher teacher)
        {
            InitializeComponent();

            HasAddFunction = false;

            EH = new EntityHelper
            {
                Builder = () => null,

                IsBlank = obj => false,

                Validator = obj => (true, ""),

                Comparer = (obj1, obj2) => (true, ""),

                SearchTextMatcher = (obj, searchText) =>
                {
                    var lesson = obj as Lesson;
                    return lesson.Timetable.Group.Title.Contains(searchText)
                        || $"{lesson.Date}".Contains(searchText);
                },

                Deleter = obj =>
                {
                    var lesson = obj as Lesson;
                    lesson.IsActive = false;
                    return (true, "");
                },

                IsDeleted = obj =>
                {
                    var lesson = obj as Lesson;
                    return lesson.IsActive == false;
                }
            };

            dataGridGroupping = (
                new PropertyGroupDescription("Timetable.WeekDay.Title"),
                FindResource("LessonDataGridGroupStyle") as GroupStyle
            );

            EntitiesSource = new ObservableCollection<object>(App.DB.Lessons.Local.Where(lesson => lesson.Timetable.Group.Teacher == teacher));

            HasEditFunction = true;
            EditHandler += () =>
            {
                if (InnerDataGrid.SelectedItem == null)
                    return;
                var selectedLesson = InnerDataGrid.SelectedItem as Lesson;
                var lessonEditWindow = new LessonEditWindow(selectedLesson);
                lessonEditWindow.ShowDialog();
                lessonEditWindow.Activate();
            };
        }
    }
}
