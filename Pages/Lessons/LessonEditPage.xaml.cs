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
    /// Логика взаимодействия для LessonEditPage.xaml
    /// </summary>
    public partial class LessonEditPage : EntityPage
    {
        public LessonEditPage(Lesson lesson)
        {
            InitializeComponent();

            EH = new EntityHelper
            {
                Builder = () =>
                {
                    var newLesson_Pupil = new Lesson_Pupil
                    {
                        Pupil = null,
                        WasInClass = false
                    };
                    lesson.Lesson_Pupil.Add(newLesson_Pupil);
                    return newLesson_Pupil;
                },

                IsBlank = (obj) => false,

                Validator = (obj) => (true, ""),

                Comparer = (obj1, obj2) => (true, ""),

                SearchTextMatcher = (obj, searchText) =>
                {
                    var lesson_pupil = obj as Lesson_Pupil;
                    return lesson_pupil.Pupil.FullName.Contains(searchText);
                },

                Deleter = (obj) => (true, ""),

                IsDeleted = (obj) => false
            };

            EntitiesSource = new ObservableCollection<object>(lesson.Lesson_Pupil);
        }
    }
}
