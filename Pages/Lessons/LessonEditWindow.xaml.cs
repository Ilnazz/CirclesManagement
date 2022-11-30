using CirclesManagement.Components;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для LessonEditWindow.xaml
    /// </summary>
    public partial class LessonEditWindow : Window
    {
        public Lesson CurrentLesson
        {
            get { return (Lesson)GetValue(CurrentLessonProperty); }
            set { SetValue(CurrentLessonProperty, value); }
        }
        public static readonly DependencyProperty CurrentLessonProperty =
            DependencyProperty.Register("CurrentLesson", typeof(Lesson), typeof(LessonEditWindow), new PropertyMetadata(null));


        public LessonEditWindow(Lesson lesson)
        {
            InitializeComponent();
            CurrentLesson = lesson;

            var lessonEditPage = new LessonEditPage(lesson);

            BtnSaveChanges.Click += (s, e) => lessonEditPage.SaveChanges();

            FrameForPage.Navigate(lessonEditPage);
        }
    }
}
